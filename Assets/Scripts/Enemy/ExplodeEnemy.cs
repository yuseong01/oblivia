using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemy : BaseEnemy<ExplodeEnemy>, IExplodable, ISpawnable
{
    [SerializeField] private GameObject _spawnPrefab;
    [SerializeField] private int _spawnCount = 4;
    [SerializeField] private float _spawnRadius = 1f;
    public int GetSpawnCount() => _spawnCount;

    public GameObject GetSpawnPrefab() => _spawnPrefab;

    public float GetSpawnRadius() => _spawnRadius;


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(_innerCollider, other, true);
            PlayerStatHandler playerStatHandler = other.GetComponent<PlayerStatHandler>();
            if (playerStatHandler != null)
            {
                playerStatHandler.Health = -GetAttackPower();
            }
            Explode();
        }
    }

    // 폭발되면 주변에 미니언 생기게
    public void Explode()
    {
        Vector3 center = transform.position;

        for (int i = 0; i < _spawnCount; i++)
        {
            float angle = (360f / _spawnCount) * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * _spawnRadius;
            Vector3 spawnPos = center + offset;

            var minon = PoolManager.Instance.Get<MinionEnemy>("Minion");
            minon.transform.position = spawnPos;
        }

        // 자신 제거
        ReturnToPool();
    }
}

