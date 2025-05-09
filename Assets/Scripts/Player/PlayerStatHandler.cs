using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{

    // 공격 주기 관련 공식 :   attack Delay = 16 - (rate * 1.3), 추후 statHandler에 넣기
    // 공격 데미지 관련 공식 : FinalDamge = 1 + (Damage -1) * 1.2f
    [Header("Attack")]
    // 공격력
    [SerializeField] private float _damage = 1.0f;
    public float Damage
    {
        get => 1 + (_damage - 1) * 1.2f;
        set => _damage += value;
    }
    // 발사 주기
    [SerializeField] private float _attackRate = 1.0f;
    public float AttackRate
    {
        get => (Mathf.Max(5, 20 - (_attackRate) * 1.3f) / 60 + _attackDelay);
        set => _attackRate += value;
    }
    // 발사 추가 딜레이 
    [SerializeField] private float _attackDelay = 0f;
    public float AttackDelay
    {
        get => _attackDelay;
        set => _attackDelay += value;
    }
    //탄환 속도
    [SerializeField] private float _attackSpeed = 5.0f; 
    public float AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed += value;
    }
    //공격 범위
    [SerializeField] private float _attackRange = 5.0f;
    public float AttackRange
    {
        get => _attackRange;
        set => _attackRange += value;
    }
}
