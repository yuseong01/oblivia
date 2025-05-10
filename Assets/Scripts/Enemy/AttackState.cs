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
        // �Ÿ�üũ ���� �־����� �ٽ� Move State��
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
        // Ÿ�̸� üũ �� ���� ����
        if (Time.time - _lastAttackTime < _attackCooldown)
            return;
        _lastAttackTime = Time.time;
        switch (obj.GetEnemyType())
        {
            case EnemyType.Boss:
                // ���� ����
                if (obj.GetHealth() <= 50f) // ü���� �����̸� ����ź
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
                // �븻 ����
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

    // �׳� �÷��̾����� �ٰ����� ����
    private void DoNormalAttack(T obj)
    {
        Transform player = obj.GetPlayerPosition();
        float dist = Vector2.Distance(obj.GetEnemyPosition().position, player.position);
        if (dist <= 1.2f)
        {
            // �÷��̾� ü�¿� ����
            // �䷱ �������� TakeDamage(1);
        }
    }

    // 
    private void DoRangedShoot(T obj, IRangedEnemy ranged)
    {
        GameObject prefab = ranged.GetProjectilePrefab("Forward");
        Vector3 dir = (obj.GetPlayerPosition().position - obj.GetEnemyPosition().position).normalized;

        GameObject proj = GameObject.Instantiate(prefab, obj.GetEnemyPosition().position, Quaternion.identity);
        int damage = 3; // ��: ���� ����ü�� 3 ������
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
