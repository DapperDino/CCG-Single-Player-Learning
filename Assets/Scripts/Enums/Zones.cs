using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Zones {
	None = 0,
	Hero = 1 << 0,
	Weapon = 1 << 1,
	Deck = 1 << 2,
	Hand = 1 << 3,
	Battlefield = 1 << 4,
	Secrets = 1 << 5,
	Graveyard = 1 << 6,
	Active = Hero | Battlefield
}

public static class ZonesExtensions {
	public static bool Contains (this Zones source, Zones target) {
		return (source & target) == target;
	}
}