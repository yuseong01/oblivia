using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RoomSpawn
{
    public RoomType roomType;

    // ���� Ÿ���� ������ �� �濡�� ����
    public List<EnemySpawnInfo> enemies = new();
}
