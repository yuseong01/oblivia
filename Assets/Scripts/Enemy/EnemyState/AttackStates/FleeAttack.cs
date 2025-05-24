using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FleeAttack: IAttackStrategy
{
    private float _fleeDistance = 4f;
    private float _safeDistance = 5f;
    private float _moveSpeed = 5f;
    private float _minFleeDistance = 0.2f;
    private float _fleeDuration = 2.5f; // 최대 도망 지속 시간
    private Vector2 _minBounds = new Vector2(-8f, -4f);
    private Vector2 _maxBounds = new Vector2(8f, 4f);

    public void ExecuteAttack(IEnemy enemy)
    {
        if (enemy is MonoBehaviour mb)
        {
            BoundsUtil.UpdateBoundsFromRoom(enemy, ref _minBounds, ref _maxBounds);
            mb.StartCoroutine(FleeRoutine(enemy));
        }
    }

    private IEnumerator FleeRoutine(IEnemy enemy)
    {
        Transform enemyTransform = enemy.GetEnemyPosition();
        Transform playerTransform = enemy.GetPlayerPosition();

        float elapsed = 0f;
        Vector2 enemyPos = enemyTransform.position;
        Vector2 playerPos = playerTransform.position;
        Vector2 target = GetFleeTarget(enemyPos, playerPos);

        while (elapsed < _fleeDuration)
        {
            elapsed += Time.deltaTime;

            enemyPos = enemyTransform.position;
            playerPos = playerTransform.position;

            float distToPlayer = Vector2.Distance(enemyPos, playerPos);
            float distToTarget = Vector2.Distance(enemyPos, target);

            // 거리 충분하면 종료
            if (distToPlayer >= _safeDistance)
                break;

            // 도착했으면 새로운 위치로 재설정
            if (distToTarget < 0.2f)
                target = GetFleeTarget(enemyPos, playerPos);

            // 이동
            Vector2 dir = (target - enemyPos).normalized;
            Vector2 newPos = enemyPos + dir * _moveSpeed * Time.deltaTime;
            newPos = BoundsUtil.ClampToBounds(newPos, _minBounds, _maxBounds);
            enemyTransform.position = newPos;

            // 시선 유지: 플레이어를 바라보게
            Vector3 scale = enemyTransform.localScale;
            if ((playerPos - enemyPos).x != 0)
            {
                scale.x = (playerPos.x > enemyPos.x) ? 1f : -1f;
                enemyTransform.localScale = scale;
            }

            yield return null;
        }
    }

    private Vector2 GetFleeTarget(Vector2 enemyPos, Vector2 playerPos)
    {
        Vector2 fleeDir = (enemyPos - playerPos).normalized;

        for (int i = 0; i < 10; i++)
        {
            float angle = Random.Range(0f, 360f);
            Vector2 rotatedDir = Quaternion.Euler(0, 0, angle) * fleeDir;
            Vector2 candidate = enemyPos + rotatedDir * _fleeDistance;

            if (IsValidTarget(candidate, enemyPos, playerPos))
                return BoundsUtil.ClampToBounds(candidate, _minBounds, _maxBounds);
        }

        // fallback 방향
        Vector2 fallback = enemyPos + ((fleeDir * 0.6f) + (-enemyPos.normalized) * 0.4f).normalized * (_fleeDistance * 0.5f);
        return BoundsUtil.ClampToBounds(fallback, _minBounds, _maxBounds);
    }

    private bool IsValidTarget(Vector2 target, Vector2 from, Vector2 player)
    {
        if (!BoundsUtil.IsInsideBounds(target, _minBounds, _maxBounds)) return false;

        float currDist = Vector2.Distance(from, player);
        float nextDist = Vector2.Distance(target, player);
        return nextDist > currDist + _minFleeDistance;
    }
}
