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

        // �Ÿ�üũ ���� �־����� �ٽ� Move State��
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
        // Ÿ�̸� üũ �� ���� ����
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
                            DoRadialShoot(obj, rangedBoss); // 60% Ȯ���� ����ź
                        }
                        else if (rand < 0.85f)
                        {
                            obj.ChangeState(new RushState<T>()); // 25% Ȯ���� ����
                        }
                        else
                        {
                            obj.ChangeState(new TeleportState<T>()); // 15% Ȯ���� �����̵�
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
                // �븻 ����
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

    // �׳� �÷��̾����� �ٰ����� ����
    private void DoNormalAttack(T obj)
    {
        //obj.GetAnimator()?.SetTrigger("Attack");
        Transform player = obj.GetPlayerPosition();
        float dist = Vector2.Distance(obj.GetEnemyPosition().position, player.position);
        if (dist <= 1.2f)
        {
            // �÷��̾� ü�¿� ����
            // �䷱ �������� TakeDamage(1);
        }
    }

    // ���Ÿ� ���� ����
    private void DoRangedShoot(T obj, IRangedEnemy ranged)
    {
        //obj.GetAnimator()?.SetTrigger("Attack");
        GameObject prefab = ranged.GetProjectilePrefab("Forward");
        Vector3 dir = (obj.GetPlayerPosition().position - obj.GetEnemyPosition().position).normalized;

        GameObject proj = GameObject.Instantiate(prefab, obj.GetEnemyPosition().position, Quaternion.identity);
        int damage = 3; // ������
        proj.GetComponent<ProjectileMosnter>()?.Initialize(dir, damage);
    }
    // �� ���� ������ �а� �����ϴ� ����
    private void DoRadialShoot(T obj, IRangedEnemy ranged)
    {
        //obj.GetAnimator()?.SetTrigger("Attack");
        int count = 12;
        int damage = 5;
        GameObject prefab = ranged.GetProjectilePrefab("Radial");
        for (int i = 0; i < count; i++)
        {
            // 360�� ���� �������� ��ġ�� ���� ���
            float angle = (360f / count) * i * Mathf.Deg2Rad;
            // ���� ���� ���� cos : x/ sin : y
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            GameObject proj = GameObject.Instantiate(prefab, obj.GetEnemyPosition().position, Quaternion.identity);
            proj.GetComponent<ProjectileMosnter>()?.Initialize(dir, damage);
        }
    }
}
