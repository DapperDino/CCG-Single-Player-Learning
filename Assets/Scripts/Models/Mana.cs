using UnityEngine;

namespace CCG.Models
{
    public class Mana
    {
        private int spent = 0;
        private int permanent = 0;
        private int overloaded = 0;
        private int pendingOverloaded = 0;
        private int temporary = 0;

        private const int MaxSlots = 10;

        public int Unlocked => Mathf.Min(permanent + temporary, MaxSlots);
        public int Available => Mathf.Min(permanent + temporary - spent, MaxSlots) - overloaded;
    }
}