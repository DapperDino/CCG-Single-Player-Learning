using UnityEngine;
using System;
using System.Collections;
using TheLiquidFire.Animation;

namespace TheLiquidFire.UI
{
	[RequireComponent(typeof(Panel))]
	public class TableViewCell : MonoBehaviour 
	{
		#region Fields
		public Panel panel { get; private set; }
		public LayoutAnchor anchor { get; private set; }
		public Panel.Position pinPosition { get; private set; }
		public Panel.Position showPosition { get; private set; }
		public Panel.Position hidePosition { get; private set; }
		#endregion

		#region MonoBehaviour
		void Awake ()
		{
			panel = GetComponent<Panel>();
			anchor = GetComponent<LayoutAnchor>();
		}
		#endregion

		#region Public
		// Used to set default anchors in case they aren't alredy configured
		public void SetShowAnchors (TextAnchor ownAnchor, TextAnchor parentAnchor)
		{
			showPosition = GetPosition("Show", ownAnchor, parentAnchor);
			pinPosition = GetPosition("Pin", ownAnchor, parentAnchor);
		}

		// Used to set default anchors in case they aren't alredy configured
		public void SetHideAnchors (TextAnchor ownAnchor, TextAnchor parentAnchor)
		{
			hidePosition = GetPosition("Hide", ownAnchor, parentAnchor);
		}

		// Called when reusing a cell for content which scrolls onto the screen
		public void Show (Vector2 offset)
		{
			showPosition.offset = offset;
			panel.SetPosition(showPosition, false);
		}

		// Called when a cell scrolls off the screen and can be reused
		public void Hide ()
		{
			panel.SetPosition(hidePosition, false);
		}

		// Called when inserting a cell between other visible content on screen
		public Tweener Insert (Vector2 offset)
		{
			showPosition.offset = hidePosition.offset = offset;
			panel.SetPosition(hidePosition, false);
			return ApplyPosition(showPosition);
		}

		// Called when a visible cell's position needs to shift due to insertion or removeal
		public void Pin (Vector2 offset)
		{
			if (panel.CurrentPosition == pinPosition)
				return;
			pinPosition.offset = offset;
			panel.SetPosition(pinPosition, false);
		}

		// Called when the table view scrolls in case a cell needs to update itself
		public Tweener Shift (Vector2 offset)
		{
			if (panel.CurrentPosition != pinPosition)
				return null;
			showPosition.offset = offset;
			return ApplyPosition(showPosition);
		}

		// Called when a cell represents data which is removed from the collection
		public Tweener Remove ()
		{
			hidePosition.offset = showPosition.offset;
			panel.SetPosition(showPosition, false);
			return ApplyPosition(hidePosition);
		}
		#endregion

		#region Private
		Tweener ApplyPosition (Panel.Position pos)
		{
			Tweener tweener = panel.SetPosition(pos, true);
			tweener.duration = 0.5f;
			return tweener;
		}

		Panel.Position GetPosition (string posName, TextAnchor ownAnchor, TextAnchor parentAnchor)
		{
			Panel.Position position = panel[posName];
			if (position == null)
			{
				position = new Panel.Position(posName, ownAnchor, parentAnchor, Vector2.zero);
				panel.AddPosition(position);
			}
			return position;
		}
		#endregion
	}
}