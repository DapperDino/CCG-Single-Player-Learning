using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana {
	public const int MaxSlots = 10;

	public int spent;
	public int permanent;
	public int overloaded;
	public int pendingOverloaded;
	public int temporary;

	public int Unlocked {
		get {
			return Mathf.Min (permanent + temporary, MaxSlots);
		}
	}

	public int Available {
		get {
			return Mathf.Min (permanent + temporary - spent, MaxSlots) - overloaded;
		}
	}
}