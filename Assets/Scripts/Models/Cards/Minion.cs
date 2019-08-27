using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Card, ICombatant, IDestructable {
	// ICombatant
	public int attack { get; set; }
	public int remainingAttacks { get; set; }
	public int allowedAttacks { get; set; }

	// IDestructable
	public int hitPoints { get; set; }
	public int maxHitPoints { get; set; }

	// Other
	public List<string> mechanics;
	public string race;
}