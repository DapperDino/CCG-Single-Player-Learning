using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionView : MonoBehaviour
{
    public Image avatar;
    public Text attack;
    public Text health;
    public Sprite inactive;
    public Sprite active;
    public Sprite inactiveTaunt;
    public Sprite activeTaunt;

    public Minion minion { get; private set; }

    public void Display(Minion minion)
    {
        if (minion == null) { return; }

        avatar.sprite = inactive;
        attack.text = minion.attack.ToString();
        health.text = minion.hitPoints.ToString();
    }
}