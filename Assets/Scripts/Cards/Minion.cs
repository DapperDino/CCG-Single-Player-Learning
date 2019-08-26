namespace CCG.Cards
{
    public class Minion : Card, ICombatant, IDamageable
    {
        public int Attack { get; set; }
        public int RemainingAttacks { get; set; }
        public int AllowedAttacks { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
    }
}
