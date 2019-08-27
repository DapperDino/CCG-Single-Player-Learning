using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.DataTypes;
using TheLiquidFire.Pooling;
using TheLiquidFire.Extensions;
using TheLiquidFire.Animation;

namespace TheLiquidFire.UI
{
	[RequireComponent(typeof(ScrollRect))]
	[RequireComponent(typeof(IntKeyedPooler))]
	public class TableView : MonoBehaviour
	{
		#region Delegates
		public Func<TableView, int, int> sizeForCellInTableView;
		public Func<TableView, int> cellCountForTableView;
		public Action<TableView, TableViewCell, int> willShowCellAtIndex;
		public Action<TableView, TableViewCell> willHideCell;
		#endregion

		#region Fields
		public int cellSize = 100;
		public Point visibleRange { get; private set; }

		ScrollRect scrollRect;
		IntKeyedPooler cellPooler;
		ISpacer spacer;
		IContainer container;
		#endregion

		#region MonoBehaviour
		void OnEnable ()
		{
			scrollRect = GetComponent<ScrollRect>();
			cellPooler = GetComponent<IntKeyedPooler>();
			scrollRect.onValueChanged.AddListener(OnScroll);
			cellPooler.didDequeueForKey = OnDidDequeueForKey;
			cellPooler.willEnqueue = OnWillEnqueue;
		}

		void OnDisable ()
		{
			scrollRect.onValueChanged.RemoveListener(OnScroll);
			cellPooler.didDequeueForKey = null;
			cellPooler.willEnqueue = null;
		}
		#endregion

		#region Event Handlers
		void OnScroll (Vector2 pos)
		{
			Refresh();
		}

		void OnDidDequeueForKey (Poolable item, int key)
		{
			TableViewCell cell = item.GetComponent<TableViewCell>();
			cell.transform.SetParent(scrollRect.content);
			cell.transform.localScale = Vector3.one;
			cell.gameObject.SetActive(true);

			container.Flow.ConfigureCell(cell, key);
			Vector2 offset = container.Flow.GetCellOffset(key);
			cell.Show(offset);

			if (willShowCellAtIndex != null)
				willShowCellAtIndex(this, cell, key);
		}

		void OnWillEnqueue (Poolable item)
		{
			TableViewCell cell = item.GetComponent<TableViewCell>();
			cell.Hide();

			if (willHideCell != null)
				willHideCell(this, cell);
		}
		#endregion

		#region Public
		public void Reload ()
		{
			Setup();
			cellPooler.EnqueueAll();
			visibleRange = new Point(int.MaxValue, int.MinValue);

			if (cellCountForTableView == null)
				return;
	 	 	int rowCount = cellCountForTableView(this);

			if (sizeForCellInTableView != null)
			{
				spacer = new NonUniformSpacer((int index) => 
					{ 
						return sizeForCellInTableView(this, index); 
					}, rowCount);
			}
			else
			{
				spacer = new UniformSpacer(cellSize, rowCount);
			}

			if (scrollRect.horizontal)
				container = new HorizontalContainer(scrollRect, spacer);
			else
				container = new VerticalContainer(scrollRect, spacer);
			
			container.AutoSize ();
			scrollRect.content.anchoredPosition = Vector2.zero;
			Refresh();
		}

		public void InsertCell (int index)
		{
			InsertCellData(index);
			ApplyNewPositions();
		}

		public void InsertCells (HashSet<int> indexSet)
		{
			List<int> indices = indexSet.ToSortedList();
			for (int i = 0; i < indices.Count; ++i)
				InsertCellData(indices[i]);
			ApplyNewPositions();
		}

		public void RemoveCell (int index)
		{
			RemoveCellData(index);
			ApplyNewPositions();
		}

		public void RemoveCells (HashSet<int> indexSet)
		{
			List<int> indices = indexSet.ToSortedList();
			for (int i = indices.Count - 1; i >= 0; --i)
				RemoveCellData(indices[i]);
			ApplyNewPositions();
		}
		#endregion

		#region Private
		void Setup ()
		{
			scrollRect = GetComponent<ScrollRect>();
			cellPooler = GetComponent<IntKeyedPooler>();
			scrollRect.onValueChanged.AddListener(OnScroll);
			cellPooler.didDequeueForKey = OnDidDequeueForKey;
			cellPooler.willEnqueue = OnWillEnqueue;
		}

		void Refresh ()
		{
			Point range = container.Flow.GetVisibleCellRange();
			if (visibleRange == range)
				return;

			// Step 1: Reclaim any cells that are out of bounds
			for (int i = visibleRange.x; i <= visibleRange.y; ++i)
			{
				if (i < range.x || i > range.y)
					cellPooler.EnqueueByKey(i);
			}

			// Step 2: Load any cells that are missing
			for (int i = range.x; i <= range.y; ++i)
			{
				cellPooler.DequeueByKey(i);
			}

			visibleRange = range;
		}

		void InsertCellView (int index)
		{
			TableViewCell cell = cellPooler.DequeueScriptByKey<TableViewCell>(index);
			Vector2 offset = container.Flow.GetCellOffset(index);
			cell.Insert(offset);
		}

		void RemoveCellView (int index)
		{
			TableViewCell cell = cellPooler.GetScript<TableViewCell>(index);
			cellPooler.Collection.Remove(index);
			Tweener tweener = cell.Remove();
			tweener.completedEvent += (object sender, EventArgs e) => 
			{
				cellPooler.EnqueueScript(cell);
			};
		}

		void InsertCellData (int index)
		{
			List<int> keys = SortedKeys();
			for (int i = keys.Count - 1; i >= 0; --i)
			{
				int key = keys[i];
				if (key < index)
					break;

				Poolable item = cellPooler.Collection[key];
				cellPooler.Collection.Remove(key);
				cellPooler.Collection.Add(key+1, item);
				Vector2 offset = container.Flow.GetCellOffset(key);
				item.GetComponent<TableViewCell>().Pin(offset);
			}

			spacer.Insert(index);

			visibleRange = container.Flow.GetVisibleCellRange();
			if (index >= visibleRange.x && index <= visibleRange.y)
				InsertCellView(index);
		}

		void RemoveCellData (int index)
		{
			List<int> keys = SortedKeys();
			for (int i = 0; i < keys.Count; ++i)
			{
				int key = keys[i];
				if (key < index)
					continue;
				else if (key == index)
					RemoveCellView(key);
				else
				{
					Poolable item = cellPooler.Collection[key];
					cellPooler.Collection.Remove(key);
					cellPooler.Collection.Add(key-1, item);
					Vector2 offset = container.Flow.GetCellOffset(key);
					item.GetComponent<TableViewCell>().Pin(offset);
				}
			}

			int height = spacer.GetSize(index);
			spacer.Remove(index);

			visibleRange = container.Flow.GetVisibleCellRange();
			for (int i = visibleRange.x; i <= visibleRange.y; ++i)
			{
				if (cellPooler.HasKey(i))
					continue;
				TableViewCell cell = cellPooler.DequeueScriptByKey<TableViewCell>(i);
				Vector2 offset = container.Flow.GetCellOffset(i, height);
				cell.Pin(offset);
			}
		}

		void ApplyNewPositions ()
		{
			container.AutoSize ();
			foreach (int key in cellPooler.Collection.Keys)
			{
				int index = key;
				Vector2 offset = container.Flow.GetCellOffset(index);
				TableViewCell cell = cellPooler.GetScript<TableViewCell>(index);
				Tweener tweener = cell.Shift(offset);
				if (tweener != null)
				{
					tweener.completedEvent += (object sender, EventArgs e) => 
					{
						if (index < visibleRange.x || index > visibleRange.y)
							cellPooler.EnqueueByKey(index);
					};
				}
			}
		}

		List<int> SortedKeys ()
		{
			List<int> keys = new List<int>(cellPooler.Collection.Keys);
			keys.Sort();
			return keys;
		}
		#endregion
	}
}