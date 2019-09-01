using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class AttackSystem : Aspect, IObserve
{

    public const string FilterAttackersNotification = "AttackSystem.ValidateAttackerNotification";
    public const string FilterTargetsNotification = "AttackSystem.ValidateTargetNotification";

    public List<Card> validAttackers { get; private set; }
    public List<Card> validTargets { get; private set; }

    public void Refresh()
    {
        var match = container.GetMatch();
        validAttackers = GetFiltered(match.CurrentPlayer, FilterAttackersNotification);
        validTargets = GetFiltered(match.OpponentPlayer, FilterTargetsNotification);
    }

    public List<Card> GetActive(Player player)
    {
        List<Card> list = new List<Card>();
        list.Add(player[Zones.Hero][0]);
        list.AddRange(player[Zones.Battlefield]);
        return list;
    }

    public void Awake()
    {
        this.AddObserver(OnValidateAttackAction, Global.ValidateNotification<AttackAction>());
        this.AddObserver(OnPerformAttackAction, Global.PerformNotification<AttackAction>(), container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnValidateAttackAction, Global.ValidateNotification<AttackAction>());
        this.RemoveObserver(OnPerformAttackAction, Global.PerformNotification<AttackAction>(), container);
    }

    List<Card> GetFiltered(Player player, string filterNotificationName)
    {
        List<Card> list = GetActive(player);
        container.PostNotification(filterNotificationName, list);
        return list;
    }

    void OnValidateAttackAction(object sender, object args)
    {
        var action = sender as AttackAction;
        if (!validAttackers.Contains(action.attacker) ||
            !validTargets.Contains(action.target))
        {
            var validator = args as Validator;
            validator.Invalidate();
        }
    }

    void OnPerformAttackAction(object sender, object args)
    {
        var action = args as AttackAction;
        var attacker = action.attacker as ICombatant;
        attacker.remainingAttacks--;
        ApplyAttackDamage(action);
        ApplyCounterAttackDamage(action);
    }

    void ApplyAttackDamage(AttackAction action)
    {
        var attacker = action.attacker as ICombatant;
        var target = action.target as IDestructable;
        var damageAction = new DamageAction(target, attacker.attack);
        container.AddReaction(damageAction);
    }

    void ApplyCounterAttackDamage(AttackAction action)
    {
        var attacker = action.target as ICombatant;
        var target = action.attacker as IDestructable;
        if (attacker != null && target != null)
        {
            var damageAction = new DamageAction(target, attacker.attack);
            container.AddReaction(damageAction);
        }
    }
}