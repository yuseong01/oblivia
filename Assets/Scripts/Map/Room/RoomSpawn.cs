using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RoomSpawn
{
    public RoomType roomType;

    // 여러 타입의 적들을 한 방에서 스폰
    public List<EnemySpawnInfo> enemies = new();
}
