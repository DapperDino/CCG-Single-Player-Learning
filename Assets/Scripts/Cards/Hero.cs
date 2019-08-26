namespace CCG.Cards
{
    public class Hero : Card, IArmourable, ICombatant, IDamageable
    {
        public int Armour { get; set; }
        public int Attack { get; set; }
        public int RemainingAttacks { get; set; }
        public int AllowedAttacks { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
    }
}
