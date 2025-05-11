using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static IEnemy;

public class MoveState<T> : IState<T>  where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _moveSpeed = 3f;
    private float _arriveDistance = 1f; // 적이 이 거리보다 가까워지면 상태 전환

    public void Enter(T obj)
    {
        //obj.GetAnimator()?.SetBool("Idle", false);
        //obj.GetAnimator()?.SetBool("Move", true);
    }

    public void Update(T obj)
    {
        Vector2 enemyPos = obj.GetEnemyPosition().position;
        Vector2 playerPos = obj.GetPlayerPosition().position;

        float dist = Vector2.Distance(enemyPos, playerPos);

        switch (obj.GetEnemyType())
        {
            case EnemyType.Normal: // 그냥 플레이어한테 다가감
                if (dist <= _arriveDistance)
                {
                    obj.ChangeState(new AttackState<T>());
                    return;
                }
                break;
            case EnemyType.Ranged:
                if (dist <= 3f) // 사정거리 도달
                {
                    obj.ChangeState(new AttackState<T>());
                    return;
                }
                break;
            case EnemyType.Elite:
                if (dist <= _arriveDistance) // 사정거리 도달
                {
                    obj.ChangeState(new AttackState<T>());
                    return;
                }
                break;
            case EnemyType.Boss:
                obj.ChangeState(new AttackState<T>());
                // 보스 로직
                break;
        }
        // 플레이어한테 다가가는 코드
        Vector2 dir = (playerPos - enemyPos).normalized;
        obj.GetEnemyPosition().position = enemyPos + dir * _moveSpeed * Time.deltaTime;
        Debug.DrawLine(enemyPos, playerPos, Color.green);

    }

    public void Exit(T obj)
    {
        //obj.GetAnimator()?.SetBool("Move", false);
    }
}
