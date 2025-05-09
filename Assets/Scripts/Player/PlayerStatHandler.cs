using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{

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
}
