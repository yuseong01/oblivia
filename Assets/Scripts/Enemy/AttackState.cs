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
                // 보스 로직
                break;
            case IEnemy.EnemyType.Normal:
                // 노말 로직
                break;
            case IEnemy.EnemyType.Flee:
                // 도망 로직
                break;
        }
    }

    public void Exit(T obj)
    {

    }
}
