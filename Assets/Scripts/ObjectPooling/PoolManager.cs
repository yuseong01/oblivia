using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoolManager : Singleton<PoolManager>
{
    //[SerializeField] private Bullet _bulletPrefab;
    //[SerializeField] private Enemy _meleeEnemyPrefab; //근거리
    //[SerializeField] private Enemy _rangedEnemyPrefab; //원거리
    [SerializeField] private FleeEnemy _fleeEnemyPrefab; //원거리
    [SerializeField] private Boss _bossPrefab; //원거리
    //[SerializeField] private Transform _bulletParent;
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
        //CreatePool<Bullet>("Bullet", _bulletPrefab, 30, _bulletParent);
        //CreatePool<EnemyMelee>("EnemyMelee", _meleeEnemyPrefab, 20, _enemyParent);
        //CreatePool<Bullet>("EnemyRanged", _rangedEnemyPrefab, 20, _enemyParent);
        
        // 1, 적 pool 에 등록
        CreatePool<FleeEnemy>("Flee", _fleeEnemyPrefab, 20, _enemyParent);
        CreatePool<Boss>("Boss", _bossPrefab, 10, _bossParent);
        // 2. 적 정보 매핑 여기까지는 잘되고 room 
        foreach (var config in _roomSpawn)
        {
            if (!_roomSpawnMap.ContainsKey(config.roomType))
            {
                Debug.Log(config.roomType + " 처음이다.");
                // 처음 등록할 경우
                _roomSpawnMap[config.roomType] = new List<List<EnemySpawnInfo>>();
            }
            Debug.Log(config.roomType +" 처음이 아니다");
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
