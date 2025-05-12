using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    // ü�� ����� UI ������ ���� �̺�Ʈ
    public event Action<float> OnHealthChanged;
    public event Action<float> OnMaxHealthChanged;



    [Header("<Speed>")]
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed += value;
    }

    [Header("<Health>")]
    // ü��, ���� ResuorceManager�� ����?
    public float limitHealth = 10;
    [SerializeField] private float _health;
    public float Health
    {
        get => _health;
        set
        {
            _health = Mathf.Min(_health + value, MaxHealth);
            OnHealthChanged?.Invoke(_health);
        }
    }
    [SerializeField] private float _maxHealth;
    public float MaxHealth
    {
        get => _maxHealth;
        set
        {
            float newHealth = Mathf.Min(_maxHealth + value, limitHealth);

            if (_maxHealth != newHealth)
            {
                _maxHealth = newHealth;
                OnMaxHealthChanged?.Invoke(_maxHealth);
            }
        }
    }


    // ���� �ֱ� ���� ���� :   attack Delay = 16 - (rate * 1.3), ���� statHandler�� �ֱ�
    // ���� ������ ���� ���� : FinalDamge = 1 + (Damage -1) * 1.2f
    [Header("<Attack>")]
    // ���ݷ�
    [SerializeField] private float _damage;
    public float Damage
    {
        get => 1 + (_damage - 1) * 1.2f;
        set => _damage += value;
    }
    // �߻� �ֱ�
    [SerializeField] private float _attackRate;
    public float AttackRate
    {
        get => (Mathf.Max(5, 20 - (_attackRate) * 1.3f) / 60 + _attackDelay);
        set => _attackRate += value;
    }
    // �߻� �߰� ������ 
    [SerializeField] private float _attackDelay;
    public float AttackDelay
    {
        get => _attackDelay;
        set => _attackDelay += value;
    }
    //źȯ �ӵ�
    [SerializeField] private float _attackSpeed;
    public float AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed += value;
    }
    //���� ����
    [SerializeField] private float _attackRange;
    public float AttackRange
    {
        get => _attackRange;
        set => _attackRange += value;
    }
    //���� ����
    [SerializeField] private int _attackCount;
    public int AttackCount
    {
        get => _attackCount;
        set => _attackCount += value;
    }
    //���� ����
    [SerializeField] private float _attackAngle;
    public float AttackAngle
    {
        get => _attackAngle;
        set => _attackAngle += value;
    }
    //�˹� �Ŀ�
    [SerializeField] private float _knockbackForce;
    public float KnockbackForce
    {
        get => _knockbackForce;
        set => _knockbackForce += value;
    }
    //���� ���� �ð�
    [SerializeField] private float _attackDuration;
    public float AttackDuration
    {
        get => _attackDuration;
        set => _attackDuration += value;
    }
    //źȯ ũ��
    [SerializeField] private float _projectileSize;
    public float ProjectileSize
    {
        get => _projectileSize;
        set => _projectileSize += value;
    }

    // ���� ���縦 ���� Clone �޼���
    public PlayerStatHandler Clone()
    {
        PlayerStatHandler clone = new PlayerStatHandler();
        clone.Damage = this.Damage;
        clone.AttackSpeed = this.AttackSpeed;
        clone.AttackDuration = this.AttackDuration;
        clone.KnockbackForce = this.KnockbackForce;
        clone.ProjectileSize = this.ProjectileSize;
        return clone;
    }
}
