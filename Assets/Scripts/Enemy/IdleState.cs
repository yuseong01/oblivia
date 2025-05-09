using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    public void Enter(T obj)
    {
        //obj.GetAnimator()?.SetBool("Move", false);
    }

    public void Update(T obj)
    {
        if (obj.CheckInPlayerInRanged())
        {
            switch (obj.GetEnemeyType())
            {
                case IEnemy.EnemyType.Flee:
                    obj.ChangeState(new FleeState<T>());
                    return;
                case IEnemy.EnemyType.Normal:
                    obj.ChangeState(new MoveState<T>());
                    return;
                // ���� ���� ���߿� �߰�
            }
        }
    }

    public void Exit(T obj)
    {

    }
}
