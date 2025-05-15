using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatHandler : MonoBehaviour
{
    // 체력 변경시 UI 변경을 위한 이벤트
    public event Action<float> OnHealthChanged;
    public event Action<float> OnMaxHealthChanged;
    public event Action OnOrbitCountChanged;



    [Header("<Speed>")]
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed += value;
    }
    [SerializeField] private float _invincibleDuration = 1.0f; // 무적 시간 (초)
    private bool _isInvincible = false;
    [Header("<Health>")]
    // 체력, 추후 ResuorceManager로 관리?
    public float limitHealth = 10;
    [SerializeField] private float _health;
    public float Health
    {
        get => _health;
        set
        {
            if (value < 0 && _isInvincible) return; // 무적 상태면 데미지 무시

            _health = Mathf.Clamp(_health + value, 0, MaxHealth);
            OnHealthChanged?.Invoke(_health);

            // 데미지를 받은 경우 무적 시간 시작
            if (value < 0)
            {
                StartCoroutine(InvincibleCoroutine());
            }

            if(_health <= 0)
            {
                SceneManager.LoadScene("Save");   
            }
        }
    }

    private IEnumerator InvincibleCoroutine()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(_invincibleDuration);
        _isInvincible = false;
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


    // 공격 주기 관련 공식 :   attack Delay = 16 - (rate * 1.3), 추후 statHandler에 넣기
    // 공격 데미지 관련 공식 : FinalDamge = 1 + (Damage -1) * 1.2f
    [Header("<Attack>")]
    // 공격력
    [SerializeField] private float _damage;
    public float Damage
    {
        get => 1 + (_damage - 1) * 1.2f;
        set => _damage += value;
    }
    // 발사 주기
    [SerializeField] private float _attackRate;
    public float AttackRate
    {
        get => (Mathf.Max(5, 20 - (_attackRate) * 1.3f) / 60 + _attackDelay);
        set => _attackRate += value;
    }
    // 발사 추가 딜레이 
    [SerializeField] private float _attackDelay;
    public float AttackDelay
    {
        get => _attackDelay;
        set => _attackDelay += value;
    }
    //탄환 속도
    [SerializeField] private float _attackSpeed;
    public float AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed += value;
    }
    //공격 범위
    [SerializeField] private float _attackRange;
    public float AttackRange
    {
        get => _attackRange;
        set => _attackRange += value;
    }
    //공격 갯수
    [SerializeField] private int _attackCount;
    public int AttackCount
    {
        get => _attackCount;
        set => _attackCount += value;
    }
    //공격 각도
    [SerializeField] private float _attackAngle;
    public float AttackAngle
    {
        get => _attackAngle;
        set => _attackAngle += value;
    }
    //넉백 파워
    [SerializeField] private float _knockbackForce;
    public float KnockbackForce
    {
        get => _knockbackForce;
        set => _knockbackForce += value;
    }
    //공격 유지 시간
    [SerializeField] private float _attackDuration;
    public float AttackDuration
    {
        get => _attackDuration;
        set => _attackDuration += value;
    }
    //탄환 크기
    [SerializeField] private float _projectileSize;
    public float ProjectileSize
    {
        get => _projectileSize;
        set => _projectileSize += value;
    }

    //탄환 크기
    [SerializeField] private int _orbitCount;
    public int OrbitCount
    {
        get => _orbitCount;
        set 
        { 
            _orbitCount += value;
            OnOrbitCountChanged.Invoke();
        }
    }

    // 깊은 복사를 위한 Clone 메서드
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
