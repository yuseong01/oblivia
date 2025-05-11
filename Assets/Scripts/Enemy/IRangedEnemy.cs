using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangedEnemy
{
    GameObject GetProjectilePrefab(string type); // 원거리/보스를 위한 투사체 프리팹
}
