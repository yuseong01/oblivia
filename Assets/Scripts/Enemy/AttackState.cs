using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    public void Enter(T obj)
    {
        obj.GetAnimator()?.SetBool("Move", false);
    }

    public void Update(T obj)
    {
        switch (obj.GetEnemeyType())
        {
            case IEnemy.EnemyType.Boss:
                // ���� ����
                break;
            case IEnemy.EnemyType.Normal:
                // �븻 ����
                break;
            case IEnemy.EnemyType.Flee:
                // ���� ����
                break;
        }
    }

    public void Exit(T obj)
    {

    }
}
