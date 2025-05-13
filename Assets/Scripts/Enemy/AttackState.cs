using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static IEnemy;

public class AttackState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _attackCooldown = 1.5f;
    private float _lastAttackTime;

    public void Enter(T obj)
    {
        //obj.GetAnimator()?.SetBool("Move", false);
    }

    public void Update(T obj)
    {
        float dist = Vector2.Distance(obj.GetEnemyPosition().position, obj.GetPlayerPosition().position);
        float rangedRange = 4f;
        float normalRange = 1.5f;

        // 거리체크 만약 멀어지면 다시 Move State로
        switch (obj.GetEnemyType())
        {
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
            case EnemyType.Teleport:
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
                if (obj is IRangedEnemy rangedBoss)
                {
                    float rand = Random.value; // 0.0 ~ 1.0

                    if (obj.GetHealth() <= 50f)
                    {
                        if (rand < 0.6f)
                        {
                            DoRadialShoot(obj, rangedBoss); // 60% 확률로 광역탄
                        }
                        else if (rand < 0.85f)
                        {
                            obj.ChangeState(new RushState<T>()); // 25% 확률로 돌진
                        }
                        else
                        {
                            obj.ChangeState(new TeleportState<T>()); // 15% 확률로 순간이동
                        }
                    }
                    else
                    {
                        if (rand < 0.3f)
                        {
                            DoRadialShoot(obj, rangedBoss);
                        }
                        else if(rand > 0.3f && rand < 0.7f)
                        {
                            obj.ChangeState(new TeleportState<T>());
                        }
                        else
                        {
                            obj.ChangeState(new RushState<T>());
                        }
                    }
                }
                break;
            case EnemyType.Clone:
                obj.ChangeState(new MoveState<T>());
                break;
            case EnemyType.Normal:
                // 노말 로직
                DoNormalAttack(obj);
                break;
            case EnemyType.Ranged:
                if (obj is IRangedEnemy ranged)
                    DoRangedShoot(obj, ranged);
                break;
            case EnemyType.Elite:
                if(obj.GetHealth() < 50)
                {
                    obj.ChangeState(new FleeState<T>());
                }
                DoNormalAttack(obj);
                break;
            case EnemyType.Teleport:
                obj.ChangeState(new TeleportState<T>());
                break;
        }
    }

    public void Exit(T obj)
    {

    }

    // 그냥 플레이어한테 다가가는 몬스터
    private void DoNormalAttack(T obj)
    {
        //obj.GetAnimator()?.SetTrigger("Attack");
        Transform player = obj.GetPlayerPosition();
        float dist = Vector2.Distance(obj.GetEnemyPosition().position, player.position);
        if (dist <= 1.2f)
        {
            // 플레이어 체력에 영향
            // 요런 느낌으로 TakeDamage(1);
        }
    }

    // 원거리 공격 몬스터
    private void DoRangedShoot(T obj, IRangedEnemy ranged)
    {
        //obj.GetAnimator()?.SetTrigger("Attack");
        GameObject prefab = ranged.GetProjectilePrefab("Forward");
        Vector3 dir = (obj.GetPlayerPosition().position - obj.GetEnemyPosition().position).normalized;

        GameObject proj = GameObject.Instantiate(prefab, obj.GetEnemyPosition().position, Quaternion.identity);
        int damage = 3; // 데미지
        proj.GetComponent<ProjectileMosnter>()?.Initialize(dir, damage);
    }
    // 더 넓은 범위로 넓게 공격하는 몬스터
    private void DoRadialShoot(T obj, IRangedEnemy ranged)
    {
        //obj.GetAnimator()?.SetTrigger("Attack");
        int count = 12;
        int damage = 5;
        GameObject prefab = ranged.GetProjectilePrefab("Radial");
        for (int i = 0; i < count; i++)
        {
            // 360도 원을 기준으로 배치할 각도 계산
            float angle = (360f / count) * i * Mathf.Deg2Rad;
            // 원형 방향 벡터 cos : x/ sin : y
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            GameObject proj = GameObject.Instantiate(prefab, obj.GetEnemyPosition().position, Quaternion.identity);
            proj.GetComponent<ProjectileMosnter>()?.Initialize(dir, damage);
        }
    }
}
