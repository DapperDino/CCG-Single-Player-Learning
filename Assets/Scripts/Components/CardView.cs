using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public Image cardBack;
    public Image cardFront;
    public Text healthText;
    public Text attackText;
    public Text manaText;
    public Text titleText;
    public Text cardText;

    public bool isFaceUp { get; private set; }
    public Card card;
    private GameObject[] faceUpElements;
    private GameObject[] faceDownElements;

    void Awake()
    {
        faceUpElements = new GameObject[] {
            cardFront.gameObject,
            healthText.gameObject,
            attackText.gameObject,
            manaText.gameObject,
            titleText.gameObject,
            cardText.gameObject
        };
        faceDownElements = new GameObject[] {
            cardBack.gameObject
        };
        Flip(isFaceUp);
    }

    public void Flip(bool shouldShow)
    {
        isFaceUp = shouldShow;
        var show = shouldShow ? faceUpElements : faceDownElements;
        var hide = shouldShow ? faceDownElements : faceUpElements;
        Toggle(show, true);
        Toggle(hide, false);
        Refresh();
    }

    void Toggle(GameObject[] elements, bool isActive)
    {
        for (int i = 0; i < elements.Length; ++i)
        {
            elements[i].SetActive(isActive);
        }
    }

    void Refresh()
    {
        if (isFaceUp == false)
            return;

        manaText.text = card.cost.ToString();
        titleText.text = card.name;
        cardText.text = card.text;

        var minion = card as Minion;
        if (minion != null)
        {
            attackText.text = minion.attack.ToString();
            healthText.text = minion.maxHitPoints.ToString();
        }
        else
        {
            attackText.text = string.Empty;
            healthText.text = string.Empty;
        }
    }
}