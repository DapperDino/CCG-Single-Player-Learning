public class Hero : Card, IArmored, ICombatant, IDestructable
{
    // IArmored
    public int armor { get; set; }

    // ICombatant
    public int attack { get; set; }
    public int remainingAttacks { get; set; }
    public int allowedAttacks { get; set; }

    // IDesructable
    public int hitPoints { get; set; }
    public int maxHitPoints { get; set; }
}