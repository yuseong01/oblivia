using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IEnemy;
[System.Serializable]
public class EnemySpawnInfo
{
    public EnemyType type; // 타입
    public int count; // 스폰 개수
    public GameObject prefab;// 해당 적 프리팹


}
