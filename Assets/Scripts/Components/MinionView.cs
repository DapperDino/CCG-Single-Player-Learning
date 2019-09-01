using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class MinionView : BattlefieldCardView
{
    public Sprite inactiveTaunt;
    public Sprite activeTaunt;

    public Minion minion { get; private set; }
    public override Card card { get { return minion; } }

    public void Display(Minion minion)
    {
        this.minion = minion;
        Refresh();
    }

    protected override void Refresh()
    {
        if (minion == null)
            return;
        if (minion.GetAspect<Taunt>() == null)
        {
            avatar.sprite = isActive ? active : inactive;
        }
        else
        {
            avatar.sprite = isActive ? activeTaunt : inactiveTaunt;
        }
        attack.text = minion.attack.ToString();
        health.text = minion.hitPoints.ToString();
    }
}