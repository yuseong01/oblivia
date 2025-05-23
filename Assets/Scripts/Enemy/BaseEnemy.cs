using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;
using static IEnemy;

public class BaseEnemy<T> : MonoBehaviour,IPoolable, IEnemy, IStateMachineOwner<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>, IPoolable
{
    protected StateMachine<T> _fsm = new StateMachine<T>();

    [Header("Enemy Settings")]
    [SerializeField] public Transform _player;
    [SerializeField, Range(0f, 200f)] protected float _health = 10f;
    [SerializeField] protected float _detectRange = 5f;
    [SerializeField] protected EnemyType _type;
    [SerializeField] protected float _speed = 3f;
    [SerializeField] protected float _attackPower = 10f;
    [SerializeField] protected Collider2D _innerCollider;

    [Header("Knockback")]
    private Vector2 _knockbackVelocity;
    private float _knockbackTimer;
    private float _knockbackDuration = 1f;
    private float _knockbackDrag = 20f;
    private bool _isKnockback;

    protected SpriteRenderer _spriteRenderer;
    public Vector2 _minBounds = new Vector2(-8, -4);
    public Vector2 _maxBounds = new Vector2(8, 4);

    protected IState<T> _currentState;
    private string _poolKey;
    private Room _currentRoom;
    private bool _isDead = false;
    protected Animator _anim;
    private EnemyHitEffect _hitEffect;
    private bool _isStop= false;
    protected virtual void Awake()
    {
        Debug.Log(_type);
        _anim = GetComponent<Animator>();
        _poolKey = _type.ToString();
        _spriteRenderer= gameObject.GetComponent<SpriteRenderer>();
        _hitEffect = GetComponent<EnemyHitEffect>();
    }

    private void Start()
    {
        ChangeState(new IdleState<T>());
        _player = PlayerManager.Instance.PlayerTransform();
    }
    protected virtual void Update()
    {
        if (_isStop) return;
        HandleKnockback();
        if (!_isKnockback) 
            _fsm.Update(this as T);
    }
    public void SetStop(bool stop)
    {
        _isStop = stop;
    }
    private void HandleKnockback()
    {
        if (!_isKnockback) return;

        _knockbackTimer += Time.deltaTime;
        _knockbackVelocity = Vector2.Lerp(_knockbackVelocity, Vector2.zero, _knockbackDrag * Time.deltaTime);
        transform.position += (Vector3)(_knockbackVelocity * Time.deltaTime);

        if (_knockbackTimer >= _knockbackDuration)
        {
            _isKnockback = false;
            _knockbackVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerStatHandler playerStatHandler = other.GetComponent<PlayerStatHandler>();
            if (playerStatHandler != null)
            {
                playerStatHandler.Health = -GetAttackPower();
            }
        }

        if (other.CompareTag("PlayerBullet"))
        {
            Vector2 hitDirection = (transform.position -_player.position).normalized;
            TakeDamage(1, hitDirection);
        }
    }

    public void ChangeState(IState<T> newState)
    {
        _fsm.ChangeState(newState, this as T);
        _currentState = newState;
    }

    // IEnemy ����
    public Transform GetPlayerPosition()
    {
        if (_player == null)
            _player = PlayerManager.Instance.PlayerTransform();
        return _player;
    }
    public float GetPlayerHealth() => _health;
    public bool CheckInPlayerInRanged()
    {
        if (_player == null)
            _player = PlayerManager.Instance.PlayerTransform();
        return Vector3.Distance(transform.position, _player.position) < _detectRange;
    }
    public EnemyType GetEnemyType() => _type;
    public Animator GetAnimator() => _anim;
    public Transform GetEnemyPosition() => transform;
    public float GetHealth() => _health;
    public float SetSpeed(float amount) => _speed = amount;
    public float GetSpeed() => _speed;
    public void TakeDamage(float amount, Vector2 hitDirection) // 몬스터가 공격을 받는 거
    {
        if (_isDead) return;
        _health -= amount;
        //ChangeState(new KnockbackState<T>());
       // Knockback(hitDirection);
        if (_hitEffect != null)
            _hitEffect.PlayEffect();

        if (_health <= 0f)
        {
            Debug.Log("죽음");
            if(_type != EnemyType.Explode)
                isDie();
        }
    }
    public void isDie()
    {
        _isDead = true;
        ChallengeManager.Instance.IncreaseProgress("kill_monsters", 1);
        _currentRoom?.EnemyDied();
        ChangeState(new DieState<T>(GetEnemyType().ToString(), _type));
        Invoke("ReturnToPool",1f);
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
        _speed = UnityEngine.Random.Range(1f, 2f); // 여기에 원하는 범위 설정
        _isDead = false;
        _player = PlayerManager.Instance.PlayerTransform();
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
            case EnemyType.Rush1:
                PoolManager.Instance.Return(_poolKey, this as RushEnemy);
                break;
            case EnemyType.Rush2:
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

    public void Knockback(Vector2 direction, float power = 15f, float duration = 0.1f, float drag = 20f)
    {
        _knockbackVelocity = direction.normalized * power;
        _knockbackTimer = 0f;
        _knockbackDuration = duration;
        _knockbackDrag = drag;
        _isKnockback = true;
    }
}
