using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static IEnemy;

public class MoveState<T> : IState<T>  where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _moveSpeed;
    private float _arriveDistance = 1f; // ���� �� �Ÿ����� ��������� ���� ��ȯ
    private float _distance = 1f;
    public void Enter(T obj)
    {
        obj.GetAnimator()?.CrossFade("Move", 0f);
        _moveSpeed = obj.GetSpeed();
    }

    public void Update(T obj)
    {
        Vector2 enemyPos = obj.GetEnemyPosition().position;
        Vector2 playerPos = obj.GetPlayerPosition().position;

        float dist = Vector2.Distance(enemyPos, playerPos);

        switch (obj.GetEnemyType())
        {
            case EnemyType.Teleport:
                obj.ChangeState(new AttackState<T>());
                break;
            default:
                if (dist <= _arriveDistance)
                {
                    obj.ChangeState(new AttackState<T>());
                    return;
                }
                break;
        }
        // �ٶ󺸴� ���� ����
        var sr = (obj as MonoBehaviour).GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = playerPos.x < enemyPos.x;
        }

        // �÷��̾���� ���� ���� �ڵ�
        Vector2 playerDistance = playerPos - enemyPos;
        float distance = playerDistance.magnitude;
        if (EnemyType.Minion != obj.GetEnemyType())
        {
            // �÷��̾����� �ٰ����� �ڵ�
            Vector2 dir = (playerPos - enemyPos).normalized;
            obj.GetEnemyPosition().position = enemyPos + dir * _moveSpeed * Time.deltaTime;
        }
        else
        {
            if (distance > _distance)
            {
                Vector2 dir = (playerPos - enemyPos).normalized;
                obj.GetEnemyPosition().position = enemyPos + dir * _moveSpeed * Time.deltaTime;

            }
        }
        Debug.DrawLine(enemyPos, playerPos, Color.green);
    }

    public void Exit(T obj)
    {
        //obj.GetAnimator()?.SetBool("Move", false);
    }
}
