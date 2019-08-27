using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.Animation;
using TheLiquidFire.Pooling;

public class HandView : MonoBehaviour
{
    public List<Transform> cards = new List<Transform>();
    public Transform activeHandle;
    public Transform inactiveHandle;

    public IEnumerator AddCard(Transform card, bool showPreview, bool overDraw)
    {
        if (showPreview)
        {
            var preview = ShowPreview(card);
            while (preview.MoveNext())
                yield return null;
        }

        if (overDraw)
        {
            var discard = OverdrawCard(card);
            while (discard.MoveNext()) { yield return null; }
        }
        else
        {
            cards.Add(card);
            var layout = LayoutCards();
            while (layout.MoveNext())
                yield return null;
        }
    }

    IEnumerator ShowPreview(Transform card)
    {
        Tweener tweener = null;
        card.RotateTo(activeHandle.rotation);
        tweener = card.MoveTo(activeHandle.position, Tweener.DefaultDuration, EasingEquations.EaseOutBack);
        var cardView = card.GetComponent<CardView>();
        while (tweener != null)
        {
            if (!cardView.isFaceUp)
            {
                var toCard = (Camera.main.transform.position - card.position).normalized;
                if (Vector3.Dot(card.up, toCard) > 0)
                    cardView.Flip(true);
            }
            yield return null;
        }
        tweener = card.Wait(1);
        while (tweener != null)
            yield return null;
    }

    IEnumerator LayoutCards(bool animated = true)
    {
        var overlap = 0.2f;
        var width = cards.Count * overlap;
        var xPos = -(width / 2f);
        var duration = animated ? 0.25f : 0;

        Tweener tweener = null;
        for (int i = 0; i < cards.Count; ++i)
        {
            var canvas = cards[i].GetComponentInChildren<Canvas>();
            canvas.sortingOrder = i;

            var position = inactiveHandle.position + new Vector3(xPos, 0, 0);
            cards[i].RotateTo(inactiveHandle.rotation, duration);
            tweener = cards[i].MoveTo(position, duration);
            xPos += overlap;
        }

        while (tweener != null)
            yield return null;
    }

    IEnumerator OverdrawCard(Transform card)
    {
        Tweener tweener = card.ScaleTo(Vector3.zero, 0.5f, EasingEquations.EaseInBack);
        while (tweener != null)
            yield return null;
        card.gameObject.SetActive(false);
        card.localScale = Vector3.one;
        var poolable = card.GetComponent<Poolable>();
        var pooler = GetComponentInParent<BoardView>().cardPooler;
        pooler.Enqueue(poolable);
    }
}