using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAttack : IAttackStrategy
{
    private float _rushSpeed = 10f;             
    private float _rushDuration = 0.8f;

    private float _prepDelay = 0.5f;

    private float _retreatSpeed = 5f;             
    private float _retreatDuration = 1f;

    private float _waitBetween = 1.2f;

    private Vector2 _minBounds = new Vector2(-8, -4);
    private Vector2 _maxBounds = new Vector2(8, 4);

    public void ExecuteAttack(IEnemy enemy)
    {
        if (enemy is MonoBehaviour mb)
        {
            BoundsUtil.UpdateBoundsFromRoom(enemy, ref _minBounds, ref _maxBounds);
            mb.StartCoroutine(RushRoutine(enemy));
        }
    }

    private IEnumerator RushRoutine(IEnemy enemy)
    {
        Transform enemyTransform = enemy.GetEnemyPosition();
        Transform playerTransform = enemy.GetPlayerPosition();

        float dist = Vector3.Distance(enemyTransform.position, playerTransform.position);
        if (enemy.CheckInPlayerInRanged())
        {
            Vector3 awayDir = (enemyTransform.position - playerTransform.position).normalized;
            yield return DoMove(enemyTransform, awayDir, _retreatSpeed, 0.5f);
            yield break;
        }

        // 돌진 준비
        yield return new WaitForSeconds(_prepDelay);

        // 플레이어 돌진
        yield return DoRush(enemyTransform, playerTransform.position, _rushSpeed, _rushDuration);

        // 후퇴
        Vector3 retreatDir = (enemyTransform.position - playerTransform.position).normalized;
        yield return DoMove(enemyTransform, retreatDir, _retreatSpeed, _retreatDuration);

        // 대기
        yield return new WaitForSeconds(_waitBetween);
    }

    private IEnumerator DoRush(Transform enemyTransform, Vector3 targetPos, float speed, float duration)
    {
        float elapsed = 0f;
        float acceleration = 40f;
        float currentSpeed = 0f;
        Vector3 dir = (targetPos - enemyTransform.position).normalized;

        // 스프라이트 방향 반전
        if (dir.x != 0)
        {
            Vector3 scale = enemyTransform.localScale;
            scale.x = dir.x > 0 ? -1f : 1f;
            enemyTransform.localScale = scale;
        }

        while (elapsed < duration)
        {
            Vector3 toPlayer = targetPos - enemyTransform.position;

            // 너무 가까워지면 멈춤
            if (toPlayer.magnitude < 0.5f)
                break;

            // 가속 적용
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, _rushSpeed);

            // 이동
            Vector3 newPos = enemyTransform.position + dir * currentSpeed * Time.deltaTime;
            newPos = BoundsUtil.ClampToBounds(newPos, _minBounds, _maxBounds);
            enemyTransform.position = newPos;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator DoMove(Transform enemyTransform, Vector3 dir, float speed, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 newPos = enemyTransform.position + dir * speed * Time.deltaTime;
            newPos = BoundsUtil.ClampToBounds(newPos, _minBounds, _maxBounds);
            enemyTransform.position = newPos;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
