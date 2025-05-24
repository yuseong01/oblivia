using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour, IAttackStrategy
{
    private float _attackRange = 1.5f;
    private float _moveSpeed = 2f;

    public void ExecuteAttack(IEnemy enemy)
    {
        if (enemy is MonoBehaviour mb)
        {
            mb.StartCoroutine(ApproachAndAttack(enemy));
        }
    }

    private IEnumerator ApproachAndAttack(IEnemy enemy)
    {
        Transform enemyTransform = enemy.GetEnemyPosition();
        Transform player = enemy.GetPlayerPosition();

        while (true)
        {
            if (player == null)
                yield break;

            float dist = Vector2.Distance(enemyTransform.position, player.position);

            // 1. 플레이어에게 이동
            if (dist > _attackRange)
            {
                Vector3 direction = (player.position - enemyTransform.position).normalized;
                enemyTransform.position += direction * _moveSpeed * Time.deltaTime;

                // 방향 전환 (스프라이트 반전)
                if (direction.x != 0)
                {
                    Vector3 scale = enemyTransform.localScale;
                    scale.x = direction.x > 0 ? -1 : 1;
                    enemyTransform.localScale = scale;
                }

                yield return null;
            }
        }
    }
}
