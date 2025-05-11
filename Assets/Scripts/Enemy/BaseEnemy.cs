using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static IEnemy;

public class BaseEnemy<T> : MonoBehaviour, IEnemy, IStateMachineOwner<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    protected StateMachine<T> _fsm = new StateMachine<T>();

    [Header("Enemy Settings")]
    [SerializeField] protected Transform _player;
    [SerializeField, Range(1f, 100f)] protected float _health = 100f;
    [SerializeField] protected float _detectRange = 5f;
    [SerializeField] protected EnemyType _type = EnemyType.Normal;
    protected Animator _anim;
    // Unity �ʱ�ȭ
    protected virtual void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        _fsm.ChangeState(new IdleState<T>(), this as T); // T = ����� Enemy Ÿ��
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
    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0f)
        {
            ChangeState(new DieState<T>());
        }
    }
}
