using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    public void Enter(T obj)
    {
        //obj.GetAnimator()?.SetBool("Move", false);
        //obj.GetAnimator()?.SetBool("Idle", true);
    }

    public void Update(T obj)
    {
        if (obj.CheckInPlayerInRanged())
        {
            switch (obj.GetEnemyType())
            {
                case IEnemy.EnemyType.Flee:
                    obj.ChangeState(new FleeState<T>());
                    return;
                case IEnemy.EnemyType.Normal:
                    obj.ChangeState(new MoveState<T>());
                    return;
                case IEnemy.EnemyType.Ranged:
                    obj.ChangeState(new MoveState<T>());
                    return;
                case IEnemy.EnemyType.Elite:
                    obj.ChangeState(new MoveState<T>());
                    return;
                case IEnemy.EnemyType.Boss:
                    obj.ChangeState(new AttackState<T>());
                    return;
                case IEnemy.EnemyType.Teleport:
                    obj.ChangeState(new TeleportState<T>());
                    return;
                case IEnemy.EnemyType.Rush:
                    obj.ChangeState(new MoveState<T>());
                    return;
                case IEnemy.EnemyType.Explode:
                    obj.ChangeState(new MoveState<T>());
                    return;
                case IEnemy.EnemyType.Minion:
                    obj.ChangeState(new MoveState<T>()); 
                    return;
            }
        }
    }

    public void Exit(T obj)
    {

    }
}
