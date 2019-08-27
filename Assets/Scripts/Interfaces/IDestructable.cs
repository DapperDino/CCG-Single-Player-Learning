using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructable {
	int hitPoints { get; set; }
	int maxHitPoints { get; set; }
}