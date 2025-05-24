using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialAttack : IAttackStrategy
{
    public void ExecuteAttack(IEnemy enemy)
    {
        if (enemy is IRangedEnemy ranged)
        {
            GameObject prefab = ranged.GetProjectilePrefab("Radial");
            for (int i = 0; i < 12; i++)
            {
                float angle = (360f / 12) * i * Mathf.Deg2Rad;
                Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
                GameObject proj = GameObject.Instantiate(prefab, enemy.GetEnemyPosition().position, Quaternion.identity);
                proj.GetComponent<ProjectileMosnter>()?.Initialize(dir, 5);
            }
        }
    }
}
