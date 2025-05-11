using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObstacle : BaseObstacle, ISpawnable
{
    [Header("���� or ����")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _rewardPrefab;
    [SerializeField] private int _spawnCount = 2;
    [SerializeField] private float _spawnRadius = 2f;

    [SerializeField] private int _health = 1; // ü�� ����

    private void Awake()
    {
        _type = ObstacleType.Breakable;
    }

    public override void OnInteract(GameObject interactor)
    {
        _health--;
        if (_health <= 0)
        {
            SpawnAround(transform.position);
            Destroy(gameObject);
        }
    }
    private void SpawnAround(Vector3 center)
    {
        bool spawnType = Random.value < 0.5f;
        GameObject type = spawnType ? _enemyPrefab : _rewardPrefab;
        for (int i = 0; i < _spawnCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * _spawnRadius;
            Vector3 pos = center + new Vector3(offset.x, 0f, offset.y);
            Instantiate(type, pos, Quaternion.identity);
        }
    }

    // ISpawnable ������
    public GameObject GetSpawnPrefab() => Random.value < 0.5f ? _enemyPrefab : _rewardPrefab;
    public int GetSpawnCount() => _spawnCount;
    public float GetSpawnRadius() => _spawnRadius;

}
