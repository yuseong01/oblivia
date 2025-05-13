using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static IEnemy;

public class BaseEnemy<T> : MonoBehaviour,IPoolable, IEnemy, IStateMachineOwner<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>, IPoolable
{
    protected StateMachine<T> _fsm = new StateMachine<T>();

    [Header("Enemy Settings")]
    [SerializeField] public Transform _player;
    [SerializeField, Range(0f, 100f)] protected float _health = 100f;
    [SerializeField] protected float _detectRange = 5f;
    [SerializeField] protected EnemyType _type = EnemyType.Normal;
    [SerializeField] protected float _speed = 3f;
    private string _poolKey;
    protected Animator _anim;
    // Unity �ʱ�ȭ
    protected virtual void Awake()
    {
        _anim = GetComponent<Animator>();
        _poolKey = _type.ToString();

    }
    private void Start()
    {
        ChangeState(new IdleState<T>());
        // Die 확인용
        //ChangeState(new DieState<T>(_type.ToString()));
    }
    protected virtual void Update()
    {
        _fsm.Update(this as T);
    }

    // ���� ���� �Լ�
    public void ChangeState(IState<T> newState)
    {
        _fsm.ChangeState(newState, this as T);
    }

    // IEnemy ����
    public Transform GetPlayerPosition() => _player;
    public float GetPlayerHealth() => _health;
    public bool CheckInPlayerInRanged() => Vector3.Distance(transform.position, _player.position) < _detectRange;
    public EnemyType GetEnemyType() => _type;
    public Animator GetAnimator() => _anim;
    public Transform GetEnemyPosition() => transform;
    public float GetHealth() => _health;
    public float SetSpeed(float amount) => _speed = amount;
    public float GetSpeed() => _speed;
    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0f)
        {
            ChangeState(new DieState<T>(_type.ToString()));
        }
    }

    public void OnSpawned()
    {
        gameObject.SetActive(true);
        _player = GameObject.FindWithTag("Player").transform;
        _health = 100;
        if (_type == EnemyType.Boss)
            _fsm.ChangeState(new CloneState<T>(), this as T);
        else _fsm.ChangeState(new IdleState<T>(), this as T); // T = ����� Enemy Ÿ��
    }
    public void OnDespawned()
    {
        gameObject.SetActive(false);
    }

    public void ReturnToPool()
    {
        switch (_type)
        {
            case EnemyType.Flee:
                PoolManager.Instance.Return(_poolKey, this as FleeEnemy);
                break;
            case EnemyType.Normal:
                PoolManager.Instance.Return(_poolKey, this as MoveEnemy);
                break;
            case EnemyType.Boss:
                PoolManager.Instance.Return(_poolKey, this as Boss);
                break;
            default:
                break;
        }
    }
}
