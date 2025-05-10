using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    [Header("Speed")]

    [Header("Health")]
    // ü��, ���� ResuorceManager�� ����?
    [SerializeField] private float _health = 100;
    public float Health
    {
        get => _health;
        set => _health = Mathf.Min(_health + value, MaxHealth);
    }
    [SerializeField] private float _maxHealth = 100;
    public float MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth += value;
    }


    // ���� �ֱ� ���� ���� :   attack Delay = 16 - (rate * 1.3), ���� statHandler�� �ֱ�
    // ���� ������ ���� ���� : FinalDamge = 1 + (Damage -1) * 1.2f
    [Header("Attack")]
    // ���ݷ�
    [SerializeField] private float _damage = 1.0f;
    public float Damage
    {
        get => 1 + (_damage - 1) * 1.2f;
        set => _damage += value;
    }
    // �߻� �ֱ�
    [SerializeField] private float _attackRate = 1.0f;
    public float AttackRate
    {
        get => (Mathf.Max(5, 20 - (_attackRate) * 1.3f) / 60 + _attackDelay);
        set => _attackRate += value;
    }
    // �߻� �߰� ������ 
    [SerializeField] private float _attackDelay = 0f;
    public float AttackDelay
    {
        get => _attackDelay;
        set => _attackDelay += value;
    }
    //źȯ �ӵ�
    [SerializeField] private float _attackSpeed = 5.0f; 
    public float AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed += value;
    }
    //���� ����
    [SerializeField] private float _attackRange = 5.0f;
    public float AttackRange
    {
        get => _attackRange;
        set => _attackRange += value;
    }
    //���� ����
    [SerializeField] private int _attackCount = 1;
    public int AttackCount
    {
        get => _attackCount;
        set => _attackCount += value;
    }
    //���� ����
    [SerializeField] private float _attackAngle = 10.0f;
    public float AttackAngle
    {
        get => _attackAngle;
        set => _attackAngle += value;
    }
    //�˹� �Ŀ�
    [SerializeField] private float _knockbackForce = 10.0f;
    public float KnockbackPower
    {
        get => _knockbackForce;
        set => _knockbackForce += value;
    }
    //���� ���� �ð�
    [SerializeField] private float _attackDuration = 3.0f;
    public float AttackDuration
    {
        get => _attackDuration;
        set => _attackDuration += value;
    }
    //źȯ ũ��
    [SerializeField] private float _projectileSize = 0.2f;
    public float ProjectileSize
    {
        get => _projectileSize;
        set => _projectileSize += value;
    }
}
