using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpellAction : GameAction {
	public Spell spell;

	public CastSpellAction (Spell spell) {
		this.spell = spell;
	}
}
