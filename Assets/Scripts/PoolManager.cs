using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoolManager : Singleton<PoolManager>
{
    //[SerializeField] private Bullet _bulletPrefab;
    //[SerializeField] private Enemy _meleeEnemyPrefab; //근거리
    //[SerializeField] private Enemy _rangedEnemyPrefab; //원거리

    [SerializeField] private Transform _bulletParent;
    [SerializeField] private Transform _enemyParent;

    private Dictionary<string, object> pools = new();
    
    protected override void Awake()
    {
        base.Awake();
        //CreatePool<Bullet>("Bullet", _bulletPrefab, 30, _bulletParent);
        //CreatePool<EnemyMelee>("EnemyMelee", _meleeEnemyPrefab, 20, _enemyParent);
        //CreatePool<Bullet>("EnemyRanged", _rangedEnemyPrefab, 20, _enemyParent);
    }

    public void CreatePool<T>(string key, T prefab, int count, Transform parent)
        where T : MonoBehaviour, IPoolable
    {
        var pool = new ObjectPool<T>(prefab, count, parent);
        pools.Add(key, pool);
    }
}
