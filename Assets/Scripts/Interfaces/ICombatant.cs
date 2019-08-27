using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatant {
	int attack { get; set; }
	int remainingAttacks { get; set; }
	int allowedAttacks { get; set; }
}