using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static IEnemy;

public class AttackState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _lastAttackTime;
    private float _cooldown = 0.5f;

    public void Enter(T obj)
    {
        obj.GetAnimator()?.CrossFade("Attack", 0f);
        TryAttack(obj);
    }

    public void Update(T obj)
    {
        EnemyType type = obj.GetEnemyType();
        float distanceToPlayer = Vector2.Distance(obj.GetEnemyPosition().position, obj.GetPlayerPosition().position);
        bool isRush = type == EnemyType.Rush1 || type == EnemyType.Rush2;

        // 상태 유지 조건: Rush는 멀리 가면 Move, 일반은 가까움 or 멀어지면 Move
        if ((isRush && distanceToPlayer > 6f) || (!isRush && (distanceToPlayer > 5f || distanceToPlayer < 1f)))
        {
            obj.ChangeState(new MoveState<T>());
            return;
        }

        if (Time.time - _lastAttackTime >= _cooldown)
        {
            _lastAttackTime = Time.time;
            TryAttack(obj);
        }
    }

    private void TryAttack(T obj)
    {
        if (AttackDescison<T>.TryDecideAttack(obj, out var behavior))
            behavior.ExecuteAttack(obj);
    }

    public void Exit(T obj) { }
}
