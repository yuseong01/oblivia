# 유니티 입문 10기_1조 TextRPG 팀 프로젝트 입니다.

# 레퍼런스 게임을 바탕으로 우리만의 게임 제작하기! - 궁수의 전설

스파르타 코딩클럽 10기, 유니티 입문 팀 프로젝트를 진행했습니다.

## 📷 스크린샷

![메인gif](https://github.com/user-attachments/assets/1e0f4d0d-4b3a-404c-a40e-96cf2b5f60dc)

 - 게임명 : Oblivia
 - 장르 : 탄막 슈팅 류 로그라이크
 - 개발 환경 : Unity 2022.3.17f1
 - 타켓 플레폼 : Android, PC, Web
 - 개발 기간 : 2025.05.08 ~ 2025.05.15

## 스크립트 구조

![image](https://github.com/user-attachments/assets/91cb7ad8-bead-47ab-af93-ca93a31cf271)


## 🕹️ 기능
<details>
<summary><input type="checkbox" checked disabled> (필수) 1. 랜덤 방 생성 </summary>

![절차적](https://github.com/user-attachments/assets/9495cf58-4793-4fb2-9047-11c6fa68d17a)


```
    void GenerateRooms()
    {
        // 시작 지점 (0,0)
        Vector2Int currentPos = Vector2Int.zero; 
        // 방 개수 랜덤 지정
        roomCount = UnityEngine.Random.Range(8, 12);

        while (createRoomCount < roomCount)
        {
            // 방이 없는 좌표에만 방 생성
            if (!roomInstances.ContainsKey(currentPos))
            {
                GameObject newRoom = Instantiate(room, GridToWorld(currentPos), Quaternion.identity, transform);
                Room roomComponent = newRoom.GetComponent<Room>();
                Debug.Log(createRoomCount + " : " + currentPos);
                // 1. 룸 타입 설정
                // RoomType randomType = RoomType.Normal;
                // 2. 초기화
                roomComponent.Init(currentPos, RoomType.Normal);
                // 3. 바운드 설정
                roomComponent.SetMargin(new Vector2(2f, 2f)); // 강제 적용
                roomComponent.CalculateRoomBounds();         // 이후에 바운드 계산
                roomInstances[currentPos] = newRoom;
                createRoomCount++;
            }
            currentPos += GetRandomDirection();
        }
    }

```
- 절차적 생성 방식을 이용하여 방을 생성하고 시작방, 일반방, 보스방으로 나눠 몬스터를 배치하도록 했습니다.
- 각각의 좌표에 RoomPrefab을 생성하고 여기에 해당하는 Door를 연결해 플레이어가 이동 할 수 있도록 했습니다.
- 이동시 문이 닫히고, 적을 모두 잡아야 문이 열리는 방식입니다.

</details>

<details>
<summary><input type="checkbox" checked disabled> (필수) 2. 캐릭터 이동과 공격 </summary>
  
![image](https://github.com/user-attachments/assets/dc11caf8-e7bf-42cd-b5b8-b1cfb0498d9e)


```

    void Update()
    {
        Vector2 input = new Vector2(joystick.horizontal, joystick.vertical);

        float magnitude = Mathf.Min(input.magnitude / joystick.stickRange, 1f);

        if (magnitude < deadZone)
            magnitude = 0f;


        Vector2 ratioInput = input.normalized * magnitude;
        transform.position += (Vector3)(ratioInput * speed * Time.deltaTime);

        if (ratioInput.x != 0)
        {
            _spriteRenderer.flipX = ratioInput.x < 0;
        }
        if (ratioInput != Vector2.zero)
        {
            PlayAnimation("Walk");
        }
        else
        {
           // PlayAnimation("Idle");

            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
            _movement.Normalize();

            // 좌우 방향에 따라 스프라이트 반전
            if (_movement.x != 0)
            {
                _spriteRenderer.flipX = _movement.x < 0;
            }


            // 애니메이션 전환
            if (_movement != Vector2.zero)
            {
                PlayAnimation("Walk");
            }
            else
            {
                PlayAnimation("Idle");
            }
        }
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * _playerStatHandler.MoveSpeed * Time.fixedDeltaTime);
    }

```
- PC 빌드를 위한 키 입력과 모바일 빌드를 위한 버튼, 가상 조이스틱을 구성했습니다.
- 이동에 따라 flip.x 변경과 애니메이션 변경이 이루어집니다.
- 추후에 리팩토링을 한다면 이동 방식을 빌드 타입에 따라 나누고, PC 버전은 뉴 인풋 시스템을 사용하도록 변경 해볼 예정입니다.

</details>

  
<details>
<summary><input type="checkbox" checked disabled> (필수) 3. 적 AI와 공격 패턴 </summary>

![KakaoTalk_20250515_013447262](https://github.com/user-attachments/assets/49f5e386-6041-4996-b815-2654ef7568bf)

![Animation](https://github.com/user-attachments/assets/6d58f12b-0695-48d4-b252-0867d88032bb)


```

public class BaseEnemy<T> : MonoBehaviour,IPoolable, IEnemy, IStateMachineOwner<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>, IPoolable
{
    protected StateMachine<T> _fsm = new StateMachine<T>();

    [Header("Enemy Settings")]
    [SerializeField] public Transform _player;
    [SerializeField, Range(0f, 200f)] protected float _health = 10f;
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
    }
    protected virtual void Update()
    {
        _fsm.Update(this as T);
        _player = GameObject.FindWithTag("Player").transform;
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
            TakeDamage(1); 
        }
    }

    public void ChangeState(IState<T> _currentState)
    {
        _fsm.ChangeState(_currentState, this as T);
    }

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
        _speed = UnityEngine.Random.Range(1f, 2f); // 여기에 원하는 범위 설정
        _isDead = false;
        _player = GameObject.FindWithTag("Player").transform;
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

```
```
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolable
{
    Queue<T> pool = new Queue<T>();
    private T _prefab; 
    private Transform _parent;


    public ObjectPool(T prefab, int size, Transform parent = null, string poolKey = "")
    {
        this._prefab = prefab;
        this._parent= parent;

        for(int i=0; i<size; i++)
        {
            T obj = Object.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

    }

    public T Get()
    {
        if(pool.Count==0)
        {
            T objTemp = Object.Instantiate(_prefab, _parent);
            objTemp.gameObject.SetActive(true);
            return objTemp;
        }
        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        obj.OnSpawned();

        return obj;
    }

    public void Return(T obj)
    {
        obj.OnDespawned();
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}

```

- FSM과 오브젝트 풀링을 이용해 적과 탄환을 생성하고 재사용 하였습니다.
- 적의 행동은 상속과 State들을 통해 관리하여 확장에 열려있도록 했습니다.

</details>




<details>
<summary><input type="checkbox" checked disabled> (필수) 4. 스킬과 업그레이드 시스템 </summary>

![메인gif](https://github.com/user-attachments/assets/1e0f4d0d-4b3a-404c-a40e-96cf2b5f60dc)

![image](https://github.com/user-attachments/assets/7b055f36-c9a2-4739-8965-a53b0ba518c6)


```

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    private float _damage;
    private List<IProjectileModule> _modules;
    private Vector2 _direction;
    public Transform Target;
    public bool CanPenetrate;
    public  float AttackDuration;
    private PlayerStatHandler _statHandler;
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private LayerMask _wallLayers;
    public float HitCooldown = 0.2f;

    private Dictionary<Collider2D, float> _lastHitTime = new Dictionary<Collider2D, float>();

    public void Init(PlayerStatHandler statHandler, Transform enemyTransform, List<IProjectileModule> modules)
    {
        _statHandler = statHandler;
        _damage = statHandler.Damage;
        Speed = statHandler.AttackSpeed;
        _modules = modules;
        Target = enemyTransform;
        AttackDuration = statHandler.AttackDuration;
        this.transform.localScale = new Vector2(statHandler.ProjectileSize, statHandler.ProjectileSize);

        foreach (var mod in modules)
        {
            mod.OnFire(this);
        }

        Destroy(gameObject, AttackDuration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (((1 << collision.gameObject.layer) & _targetLayers) != 0)
        {
            float lastTime;
            _lastHitTime.TryGetValue(collision, out lastTime);

            if (Time.time - lastTime >= HitCooldown)
            {
                var enemy = collision.GetComponent<IEnemy>();
                enemy?.TakeDamage(_damage);
                _lastHitTime[collision] = Time.time;

                Rigidbody2D rb = collision.attachedRigidbody;
                if (rb != null)
                {
                    Vector2 knockbackDir = transform.up; 
                    rb.AddForce(knockbackDir * _statHandler.KnockbackForce, ForceMode2D.Impulse);
                }

                if (!CanPenetrate)
                    Destroy(gameObject);
            }
        }
        if (((1 << collision.gameObject.layer) & _wallLayers) != 0)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(Target == null)
        {
            Destroy(gameObject);
        }
        foreach (var mod in _modules)
        {
            mod.OnUpdate(this);
        }

        transform.position += transform.up * Speed * Time.deltaTime;
    }

}


```
- 궁수의 전설처럼 아이템을 얻을수록 효과들이 융합되는 것을 구현하기 위해 모듈 형식으로 제작했습니다
- 각각의 모듈은 탄환 생성에 관여하는 IFireModule, 탄환 발사 공식에 관여하는 IProjectileModule, 플레이어 스텟에 관여하는 IStatModule 이 있습니다.
- 아이템을 습득하게 되면 해당 아이템의 스크립터블 오브젝트에서 부착된 모듈을 플레이어에게 건내주고, 이를 Update문에서 통합 관리하여 모든 효과를 융합하여 보여줍니다.


</details>


<details>
<summary><input type="checkbox" checked disabled> (필수) 5. 보스전 </summary>

![보스방](https://github.com/user-attachments/assets/1345ce4f-4f6c-410a-81e6-91897387fbb6)


```

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : BaseEnemy<Boss>,IRangedEnemy
{
    [SerializeField] private GameObject _cloneBossPrefab;
    [Header("투사체 Prefabs")]
    [SerializeField] private GameObject _forwardProjectilePrefab;
    [SerializeField] private GameObject _radialProjectilePrefab;
    public GameObject GetClonePrefab()
    {
        return _cloneBossPrefab;
    }
    public GameObject GetProjectilePrefab(string type)
    {
        switch (type)
        {
            case "Forward":
                return _forwardProjectilePrefab;
            case "Radial":
                return _radialProjectilePrefab;
            default:
                return null;
        }
    }
}


```
- 보스전 또한 State와 Enemy 를 상속받아 구현했습니다.
- 보스는 룸 타입이 Boss여야 등장합니다.
- 보스의 패턴은 순간이동, 도망치기 탄환 발사등이 있고 각각의 패턴이 랜덤하게 등장합니다.

</details>

<details>
<summary><input type="checkbox" checked disabled> (도전) 1. 배경음악과 사운드 효과 추가 </summary>

![image](https://github.com/user-attachments/assets/fac4561a-ba42-4903-986e-74961fc807ee)


```
using UnityEngine;

public enum SFXType {Jump, Hit, Die} //임시 예시입니다 필요하신 sfx추가하시면 됩니다!
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    [SerializeField] AudioClip defaultBGMClip;

    //중복되는 사운드
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip dieClip;

    public AudioClip DefaultBGMClip => defaultBGMClip;

    private void Start()
    {
        bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        //PlayBGMSource(defalutBGMClip); //배경음 자동실행
    }
    public void PlayBGMSource(AudioClip audioClip)  //배경음악 교체시
    {
        if(audioClip==null) return;

        bgmSource.clip=audioClip;
        bgmSource.loop = true;

        bgmSource.Play();
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    //사운드만 갈경우
    public void PlaySFX(AudioClip audioClip) //효과음 교체시
    {
        if(audioClip==null) return;

        sfxSource.PlayOneShot(audioClip);
    }

    //중복되는 사운드 사용할 경우
    public void PlaySFX(SFXType type)
    {
        switch(type)
        {
            case SFXType.Jump: sfxSource.PlayOneShot(jumpClip); break;
            case SFXType.Hit:sfxSource.PlayOneShot(hitClip); break;
            case SFXType.Die:sfxSource.PlayOneShot(dieClip); break;
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}


```

- 간단한 설정창을 통해 배경음악을 추가하였습니다.
- 공격, 이동 등에 대한 효과음은 추가하지 못했습니다.

</details>

<details>
<summary><input type="checkbox" checked disabled> (도전) 2. 간단한 게임 시작 화면 </summary>


![인트로](https://github.com/user-attachments/assets/d774bc9d-962d-47ef-95b5-ad8d263e9afb)


- 게임 시작 화면과 인트로 씬을 구현했습니다.
- 인트로씬은 터치하게 되면 스킵되어 게임 시작 화면으로 바로 이동합니다.

</details>

<details>
<summary><input type="checkbox" checked disabled> (도전) 3. 빌드 및 배포 </summary>


![빌드](https://github.com/user-attachments/assets/98156c51-28cb-4d60-ab8d-1550f51cabc8)


- 모바일 PC로 빌드를 완성하였습니다.
- 해상도 대응이 완벽하진 않지만 플레이가 가능함을 확인했습니다.

</details>

<details>
<summary><input type="checkbox" checked disabled> (도전) 4. 간단한 도전과제 시스템 </summary>

![도전과재ㅔ](https://github.com/user-attachments/assets/2f8a7312-a2a6-4398-bafa-6863b19be87d)


```

using System;

[Serializable]
public enum ChallengeType
{
    CountBased, // 카운트
    ConditionBased // 조건
}

[Serializable]
public class Challenge
{
    public string id;               // 도전과제 ID
    public string description;      // 설명
    public int goal;                // 목표
    public int currentCount;        // 현재 진행도
    public bool isCompleted;        // 완료 여부
    public ChallengeType type;      // 도전과제 타입

    public string rewardCharacterId; // 이 도전과제 완료 시 해금될 캐릭터 ID
}

// 챌린지 관리자

 public void IncreaseProgress(string id, int amount)
    {
        foreach (Challenge challenge in challenges)
        {
            if (challenge.id == id && !challenge.isCompleted && challenge.type == ChallengeType.CountBased)
            {
                challenge.currentCount += amount;

                if (challenge.currentCount >= challenge.goal)
                {
                    challenge.isCompleted = true;
                    ShowReward(challenge);

                    // 도전과제 완료 시 캐릭터 해금
                    if (!string.IsNullOrEmpty(challenge.rewardCharacterId))
                    {
                        // 캐릭터 해금 요청
                        CharacterManager.Instance.UnlockCharacter(challenge.rewardCharacterId);
                    }

                }

                break;
            }
        }

        SaveChallenges();
    }
    // 사용 예시 : ChallengeManager.Instance.IncreaseProgress("kill_monsters", 1);

    public void CompleteConditionChallenge(string id)
    {
        foreach (Challenge challenge in challenges)
        {
            if (challenge.id == id && !challenge.isCompleted && challenge.type == ChallengeType.ConditionBased)
            {
                challenge.isCompleted = true;
                ShowReward(challenge);

                // 도전과제 완료 시 캐릭터 해금
                if (!string.IsNullOrEmpty(challenge.rewardCharacterId))
                {
                    // 캐릭터 해금 요청
                    CharacterManager.Instance.UnlockCharacter(challenge.rewardCharacterId);
                }

                SaveChallenges();
                break;
            }
        }
    }


```

- 특정 조건에 따라 완료되는 도전과제 기능을 추가했습니다.
- 도전과제 DB를 통해서 설명과 목표 타입, 완료 여부를 확인 할 수 있습니다.
- 도전과제가 완료되면 이를 알리는 알림창이 등장합니다.

</details>

## 🛠️ 기술 스택

- C#
- .NET Core 3.1
- Newtonsoft.Json (데이터 직렬화/역직렬화)



## 🙋 개발자 정보

 팀장 - 장유성 (몬스터 생성 로직)
- 블로그 - https://dochi-programming.tistory.com
- 깃헙 - https://github.com/yuseong01

팀원 - 김경민 (데이터, UI, 게임 설정 )
- 블로그 - https://rudals4469.tistory.com/
- 깃헙 - https://github.com/rudals446

 팀원 - 김예지 (몬스터 ai, 몬스터 소환 로직)
- 블로그 - https://code-piggy.tistory.com/
- 깃헙 - https://github.com/yejii-gi

 팀원 - 설민우 (플레이어 공격 로직, 아이템, 스킬)
- 블로그 - https://velog.io/@coolblue/posts
- 깃헙 - https://github.com/coolblue185

 팀원 - 한수정 (플레이어 로직, 맵 로직, 인트로 씬)
- 블로그 - https://hanknag.tistory.com/
- 깃헙 - https://github.com/UHANKNAG

