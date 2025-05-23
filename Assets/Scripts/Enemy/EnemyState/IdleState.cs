using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _pauseTime = 2f;
    private float _timer;
    public void Enter(T obj)
    {
        //obj.GetAnimator()?.SetBool("Move", false);
        //obj.GetAnimator()?.SetBool("Idle", true);
        obj.GetAnimator()?.CrossFade("Idle", 0f);
    }

    public void Update(T obj)
    {
        _timer += Time.deltaTime;

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
                case IEnemy.EnemyType.Elite1:
                case IEnemy.EnemyType.Elite2:
                    obj.ChangeState(new AttackState<T>());
                    return;
                case IEnemy.EnemyType.Boss:
                    obj.ChangeState(new AttackState<T>());
                    return;
                case IEnemy.EnemyType.Teleport:
                    obj.ChangeState(new TeleportState<T>());
                    return;
                case IEnemy.EnemyType.Rush1:
                case IEnemy.EnemyType.Rush2:
                    obj.ChangeState(new MoveState<T>());
                    return;
                case IEnemy.EnemyType.Explode:
                    obj.ChangeState(new MoveState<T>());
                    return;
                case IEnemy.EnemyType.Minion:
                    if (_timer >= _pauseTime)
                    {
                        obj.ChangeState(new MoveState<T>());
                    }
                    return;
            }
        }
    }

    public void Exit(T obj)
    {

    }
}
