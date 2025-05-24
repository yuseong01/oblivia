using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static IEnemy;

public class MoveState<T> : IState<T>  where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _moveSpeed = 5f;

    public void Enter(T obj)
    {
        obj.GetAnimator()?.CrossFade("Move", 0f);
    }

    public void Update(T obj)
    {
        Vector2 enemyPos = obj.GetEnemyPosition().position;
        Vector2 playerPos = obj.GetPlayerPosition().position;
        float dist = Vector2.Distance(enemyPos, playerPos);

        EnemyType type = obj.GetEnemyType();

        // 상태 전이 판단
        if (HandleByEnemyType(obj, type, dist))
            return;

        // 바라보는 방향 설정
        FacingDirection(obj, enemyPos, playerPos);

        // 플레이어에게 다가감
        MoveTowardsPlayer(obj, enemyPos, playerPos);

        Debug.DrawLine(enemyPos, playerPos, Color.green);
    }

    public void Exit(T obj)
    {
        // 애니메이션 종료 등 처리 필요 시 작성
    }

    // 몬스터 타입에 따라 거리 기준 결정
    private bool HandleByEnemyType(T obj, EnemyType type, float distanceToPlayer)
    {
        switch (type)
        {
            case EnemyType.Rush1:
            case EnemyType.Rush2:
                return HandleRushEnemy(obj, distanceToPlayer);

            case EnemyType.Boss:
                return HandleStandardEnemy(obj, distanceToPlayer, 3.5f);

            case EnemyType.Elite1:
            case EnemyType.Elite2:
                return HandleStandardEnemy(obj, distanceToPlayer, 2.5f);

            default:
                return HandleStandardEnemy(obj, distanceToPlayer, 1.5f);
        }
    }

    private bool HandleRushEnemy(T obj, float distanceToPlayer)
    {
        float rushAttackRange = 3f;

        if (distanceToPlayer <= rushAttackRange)
        {
            obj.ChangeState(new AttackState<T>());
            return true;
        }

        return false;
    }

    // 일반 적은 너무 가까우면 Flee, 공격 거리면 Attack
    private bool HandleStandardEnemy(T obj, float distanceToPlayer, float attackThreshold)
    {
        const float fleeThreshold = 0.8f;

        if (distanceToPlayer <= fleeThreshold)
        {
            obj.ChangeState(new FleeState<T>());
            return true;
        }

        if (distanceToPlayer <= attackThreshold)
        {
            obj.ChangeState(new AttackState<T>());
            return true;
        }

        return false;
    }

    private void FacingDirection(T obj, Vector2 enemyPos, Vector2 playerPos)
    {
        var sr = (obj as MonoBehaviour)?.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = playerPos.x < enemyPos.x;
        }
    }

    private void MoveTowardsPlayer(T obj, Vector2 enemyPos, Vector2 playerPos)
    {
        Vector2 dir = (playerPos - enemyPos).normalized;
        Vector2 newPos = enemyPos + dir * _moveSpeed * Time.deltaTime;
        obj.GetEnemyPosition().position = newPos;
    }
}
