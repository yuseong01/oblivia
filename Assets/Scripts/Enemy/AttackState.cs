using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IEnemy;

public class AttackState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _attackCooldown = 1.5f;
    private float _lastAttackTime;
    
    public void Enter(T obj)
    {
        //obj.GetAnimator()?.SetBool("Move", false);
        //obj.GetAnimator()?.SetTrigger("Attack");
    }

    public void Update(T obj)
    {
        float dist = Vector2.Distance(obj.GetEnemyPosition().position, obj.GetPlayerPosition().position);
        float rangedRange = 4f;
        float normalRange = 1.5f;
        Debug.Log("d");
        // 거리체크 만약 멀어지면 다시 Move State로
        switch (obj.GetEnemyType())
        {
            case EnemyType.Boss:
            case EnemyType.Ranged:
                if (dist > rangedRange)
                {
                    obj.ChangeState(new MoveState<T>());
                    return;
                }
                break;

            case EnemyType.Normal:
                if (dist > normalRange)
                {
                    obj.ChangeState(new MoveState<T>());
                    return;
                }
                break;
        }
        // 타이머 체크 및 공격 적용
        if (Time.time - _lastAttackTime < _attackCooldown)
            return;
        _lastAttackTime = Time.time;
        switch (obj.GetEnemyType())
        {
            case EnemyType.Boss:
                // 보스 로직
                if (obj.GetHealth() <= 50f) // 체력이 절반이면 광역탄
                {
                    if (obj is IRangedEnemy rangedBoss)
                        DoRadialShoot(obj, rangedBoss);
                }
                else
                {
                    if (obj is IRangedEnemy rangedBoss)
                        DoRangedShoot(obj, rangedBoss);
                }
                break;
            case EnemyType.Normal:
                // 노말 로직
                DoNormalAttack(obj);
                break;
            case EnemyType.Ranged:
                if (obj is IRangedEnemy ranged)
                    DoRangedShoot(obj, ranged);
                break;
        }
    }

    public void Exit(T obj)
    {

    }

    // 그냥 플레이어한테 다가가는 몬스터
    private void DoNormalAttack(T obj)
    {
        Transform player = obj.GetPlayerPosition();
        float dist = Vector2.Distance(obj.GetEnemyPosition().position, player.position);
        if (dist <= 1.2f)
        {
            // 플레이어 체력에 영향
            // 요런 느낌으로 TakeDamage(1);
        }
    }

    // 
    private void DoRangedShoot(T obj, IRangedEnemy ranged)
    {
        GameObject prefab = ranged.GetProjectilePrefab("Forward");
        Vector3 dir = (obj.GetPlayerPosition().position - obj.GetEnemyPosition().position).normalized;

        GameObject proj = GameObject.Instantiate(prefab, obj.GetEnemyPosition().position, Quaternion.identity);
        int damage = 3; // 예: 직선 투사체는 3 데미지
        proj.GetComponent<ProjectileMosnter>()?.Initialize(dir, damage);
    }

    private void DoRadialShoot(T obj, IRangedEnemy ranged)
    {
        int count = 12;
        int damage = 5;
        GameObject prefab = ranged.GetProjectilePrefab("Radial");
        for (int i = 0; i < count; i++)
        {
            float angle = (360f / count) * i * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            GameObject proj = GameObject.Instantiate(prefab, obj.GetEnemyPosition().position, Quaternion.identity);
            proj.GetComponent<ProjectileMosnter>()?.Initialize(dir, damage);
        }
    }
}
