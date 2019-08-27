using UnityEngine;

public class DeckView : MonoBehaviour
{
    public Transform topCard;
    [SerializeField] Transform squisher;

    public void ShowDeckSize(float size)
    {
        squisher.localScale = Mathf.Approximately(size, 0) ? Vector3.zero : new Vector3(1, size, 1);
    }
}