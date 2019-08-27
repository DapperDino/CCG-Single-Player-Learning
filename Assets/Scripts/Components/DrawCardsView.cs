using System.Collections;
using UnityEngine;
using TheLiquidFire.Notifications;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Extensions;

public class DrawCardsView : MonoBehaviour
{
    void OnEnable()
    {
        this.AddObserver(OnPrepareDrawCards, Global.PrepareNotification<DrawCardsAction>());
    }

    void OnDisable()
    {
        this.RemoveObserver(OnPrepareDrawCards, Global.PrepareNotification<DrawCardsAction>());
    }

    void OnPrepareDrawCards(object sender, object args)
    {
        var action = args as DrawCardsAction;
        action.perform.viewer = DrawCardsViewer;
    }

    IEnumerator DrawCardsViewer(IContainer game, GameAction action)
    {
        yield return true;
        var drawAction = action as DrawCardsAction;
        var boardView = GetComponent<BoardView>();
        var playerView = boardView.playerViews[drawAction.player.index];

        for (int i = 0; i < drawAction.cards.Count; ++i)
        {
            int deckSize = action.player[Zones.Deck].Count + drawAction.cards.Count - (i + 1);
            playerView.deck.ShowDeckSize((float)deckSize / Player.maxDeck);

            var cardView = boardView.cardPooler.Dequeue().GetComponent<CardView>();
            cardView.card = drawAction.cards[i];
            cardView.transform.ResetParent(playerView.hand.transform);
            cardView.transform.position = playerView.deck.topCard.position;
            cardView.transform.rotation = playerView.deck.topCard.rotation;
            cardView.gameObject.SetActive(true);

            var showPreview = action.player.mode == ControlModes.Local;
            var addCard = playerView.hand.AddCard(cardView.transform, showPreview);
            while (addCard.MoveNext())
                yield return null;
        }
    }
}