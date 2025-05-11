using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static IEnemy;

public class MoveState<T> : IState<T>  where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _moveSpeed = 3f;
    private float _arriveDistance = 1f; // ���� �� �Ÿ����� ��������� ���� ��ȯ

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
            case EnemyType.Normal: // �׳� �÷��̾����� �ٰ���
                if (dist <= _arriveDistance)
                {
                    obj.ChangeState(new AttackState<T>());
                    return;
                }
                break;
            case EnemyType.Ranged:
                if (dist <= 3f) // �����Ÿ� ����
                {
                    obj.ChangeState(new AttackState<T>());
                    return;
                }
                break;
            case EnemyType.Elite:
                if (dist <= _arriveDistance) // �����Ÿ� ����
                {
                    obj.ChangeState(new AttackState<T>());
                    return;
                }
                break;
            case EnemyType.Boss:
                obj.ChangeState(new AttackState<T>());
                // ���� ����
                break;
        }
        // �÷��̾����� �ٰ����� �ڵ�
        Vector2 dir = (playerPos - enemyPos).normalized;
        obj.GetEnemyPosition().position = enemyPos + dir * _moveSpeed * Time.deltaTime;
        Debug.DrawLine(enemyPos, playerPos, Color.green);

    }

    public void Exit(T obj)
    {
        //obj.GetAnimator()?.SetBool("Move", false);
    }
}
