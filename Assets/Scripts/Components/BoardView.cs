using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.Pooling;

public class BoardView : MonoBehaviour {
	public GameObject damageMarkPrefab;
	public List<PlayerView> playerViews;
	public SetPooler cardPooler;
    public SetPooler minionPooler;

	void Start () {
		var match = GetComponentInParent<GameViewSystem> ().container.GetMatch ();
		for (int i = 0; i < match.players.Count; ++i) {
			playerViews [i].SetPlayer (match.players[i]);
		}
	}
}