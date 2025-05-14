using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoolManager : Singleton<PoolManager>
{
    [Header("Enemy Prefab")]
    [SerializeField] private FleeEnemy _fleePrefab; // 도망치는애
    [SerializeField] private RangedEnemy _rangedPrefab; // 원거리
    [SerializeField] private MoveEnemy _movePrefab; //단거리
    [SerializeField] private Boss _bossPrefab; // 보스
    [SerializeField] private TeleportEnemy _teleportPrefab; // 텔레포트
    [SerializeField] private RushEnemy _rushPrefab; // 대쉬
    [SerializeField] private RushEnemy _rush2Prefab; // 대쉬2
    [SerializeField] private ExplodeEnemy _explodePrefab; // 대쉬2
    [SerializeField] private MinionEnemy _minionPrefab; // 대쉬2
    [Header("parnet 위치")]
    [SerializeField] private Transform _enemyParent;
    [SerializeField] private Transform _bossParent;
    [Header("Room Spawn")]
    // 방안에 어떤 애들이 있을지 설정 여러 몬스터도 허용(처음 리스트는 normal 경우의 수를 만드는거고 그 안에는 몬스터들 지정 가능)
    // 나중에 랜덤으로 type이 같은 경우 뽑음
    [SerializeField] private List<RoomSpawn> _roomSpawn;
    private Dictionary<string, object> pools = new();
    Dictionary<RoomType, List<List<EnemySpawnInfo>>> _roomSpawnMap = new();
    protected override void Awake()
    {
        base.Awake();
        
        // 1, 적 pool 에 등록 그리고 room에서 get등록 basenemy return 등록 잊지 말자
        CreatePool<Boss>("Boss", _=_bossPrefab, 10, _bossParent);
        CreatePool<RangedEnemy>("Ranged", _rangedPrefab, 20, _bossParent);
        CreatePool<MoveEnemy>("Normal", _movePrefab, 20, _bossParent);
        CreatePool<TeleportEnemy>("Teleport", _teleportPrefab, 20, _bossParent);
        CreatePool<RushEnemy>("Rush", _rushPrefab, 20, _bossParent);
        CreatePool<RushEnemy>("Rush2", _rush2Prefab, 20, _bossParent);
        CreatePool<FleeEnemy>("Flee", _fleePrefab, 20, _bossParent);
        CreatePool<ExplodeEnemy>("Explode", _explodePrefab, 20, _bossParent);
        CreatePool<MinionEnemy>("Minion", _minionPrefab, 20, _bossParent);
        // 2. 적 정보 매핑 여기까지는 잘되고 room 
        foreach (var config in _roomSpawn)
        {
            if (!_roomSpawnMap.ContainsKey(config.roomType))
            {
                // 처음 등록할 경우
                _roomSpawnMap[config.roomType] = new List<List<EnemySpawnInfo>>();
            }
            // enemies 추가
            _roomSpawnMap[config.roomType].Add(config.enemies);
        }
    }

    public void CreatePool<T>(string key, T prefab, int count, Transform parent)
        where T : MonoBehaviour, IPoolable
    {
        var pool = new ObjectPool<T>(prefab, count, parent);

        pools.Add(key, pool);
    }

    public T Get<T>(string key) where T : MonoBehaviour, IPoolable
    {
        return ((ObjectPool<T>)pools[key]).Get();
    }

    public void Return<T>(string key, T obj) where T : MonoBehaviour, IPoolable
    {
        ((ObjectPool<T>)pools[key]).Return(obj);
    }

    // 3. 예외 처리 하면서 _roomSpawnMap 가져오기
    public bool TryGetSpawnData(RoomType type, out List<List<EnemySpawnInfo>> spawnList)
    {
        return _roomSpawnMap.TryGetValue(type, out spawnList);
    }
}
