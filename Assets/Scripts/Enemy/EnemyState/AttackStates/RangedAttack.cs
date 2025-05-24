using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : IAttackStrategy
{
    public void ExecuteAttack(IEnemy enemy)
    {
        if (enemy is IRangedEnemy ranged)
        {
            GameObject projPrefab = ranged.GetProjectilePrefab("Forward");
            Vector3 dir = (enemy.GetPlayerPosition().position - enemy.GetEnemyPosition().position).normalized;

            GameObject proj = GameObject.Instantiate(projPrefab, enemy.GetEnemyPosition().position, Quaternion.identity);
            proj.GetComponent<ProjectileMosnter>()?.Initialize(dir, 3);
        }
    }
}
