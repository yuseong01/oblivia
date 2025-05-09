using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState<T> : IState<T>  where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _moveSpeed = 3f;
    private float _arriveDistance = 1f; // ���� �� �Ÿ����� ��������� ���� ��ȯ

    public void Enter(T obj)
    {
        //obj.GetAnimator()?.SetBool("Move", true);
    }

    public void Update(T obj)
    {
        Vector2 enemyPos = obj.GetEnemyPosition().position;
        Vector2 playerPos = obj.GetPlayerPosition().position;

        float dist = Vector2.Distance(enemyPos, playerPos);

        if (dist > _arriveDistance)
        {
            Vector2 dir = (playerPos - enemyPos).normalized;
            obj.GetEnemyPosition().position = enemyPos + dir * _moveSpeed * Time.deltaTime;

            // �ð�ȭ
            Debug.DrawLine(enemyPos, playerPos, Color.green);
        }
        else
        {
            obj.ChangeState(new IdleState<T>()); // �����ϸ� Idle
        }
    }

    public void Exit(T obj)
    {
        //obj.GetAnimator()?.SetBool("Move", false);
    }
}
