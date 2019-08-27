using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.Pooling;

public class BoardView : MonoBehaviour
{
    public GameObject damageMarkPrefab;
    public List<PlayerView> playerViews;
    public SetPooler cardPooler;
}