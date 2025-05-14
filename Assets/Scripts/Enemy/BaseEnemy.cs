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
    [SerializeField, Range(0f, 10f)] protected float _health = 10f;
    [SerializeField] protected float _detectRange = 5f;
    [SerializeField] protected EnemyType _type = EnemyType.Normal;
    [SerializeField] protected float _speed = 3f;
    [SerializeField] protected float _attackPower = 10f;
    [SerializeField] protected Collider2D _innerCollider;
    protected SpriteRenderer _spriteRenderer;
    public Vector2 _minBounds = new Vector2(-8, -4);
    public Vector2 _maxBounds = new Vector2(8, 4);

    protected IState<T> _currentState;
    private string _poolKey;
    private Room _currentRoom;
    private bool _isDead = false;
    protected Animator _anim;
    // Unity �ʱ�ȭ
    protected virtual void Awake()
    {
        _anim = GetComponent<Animator>();
        _poolKey = _type.ToString();
        _spriteRenderer= gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        ChangeState(new IdleState<T>());
        // Die 확인용
        //ChangeState(new DieState<T>(_type.ToString()));
    }
    protected virtual void Update()
    {
        _fsm.Update(this as T);
        _player = GameObject.FindWithTag("Player").transform;
    }

    // ���� ���� �Լ�
    public void ChangeState(IState<T> _currentState)
    {
        _fsm.ChangeState(_currentState, this as T);
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
    public void TakeDamage(float amount) // 몬스터가 공격을 받는 거
    {
        Debug.Log(_health);
        if (_isDead) return;
        _health -= amount;
        if (_health <= 0f)
        {
            ChallengeManager.Instance.IncreaseProgress("kill_monsters", 1);
            _isDead = true;
            _currentRoom?.EnemyDied();
            ChangeState(new DieState<T>(_type.ToString()));
            
        }
    }
    public Room GetCurrentRoom() => _currentRoom;
    public virtual void SetCurrentRoom(Room room)
    {
        _currentRoom = room;
    }
    public void OnSpawned()
    {
        // 초기화
        gameObject.SetActive(true);
        _speed = UnityEngine.Random.Range(3f, 13f); // 여기에 원하는 범위 설정
        _isDead = false;
        _player = GameObject.FindWithTag("Player").transform;
        _health = 10;
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
            case EnemyType.Teleport:
                PoolManager.Instance.Return(_poolKey, this as TeleportEnemy);
                break;
            case EnemyType.Ranged:
                PoolManager.Instance.Return(_poolKey, this as RangedEnemy);
                break;
            case EnemyType.Rush:
                PoolManager.Instance.Return(_poolKey, this as RushEnemy);
                break;
            case EnemyType.Minion:
                PoolManager.Instance.Return(_poolKey, this as MinionEnemy);
                break;
            case EnemyType.Explode:
                PoolManager.Instance.Return(_poolKey, this as ExplodeEnemy);
                break;
            case EnemyType.Elite1:
                PoolManager.Instance.Return(_poolKey, this as ElitEnemy);
                break;
            case EnemyType.Elite2:
                PoolManager.Instance.Return(_poolKey, this as ElitEnemy);
                break;
            default:
                break;
        }
    }
    public IState<T> CurrentState => _currentState;
    public float GetAttackPower()=> _attackPower;

    public SpriteRenderer GetSpriteRenderer()
    {
        return _spriteRenderer;
    }
}
