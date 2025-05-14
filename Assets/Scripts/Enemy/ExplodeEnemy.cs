using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemy : BaseEnemy<ExplodeEnemy>, IExplodable, ISpawnable
{
    [SerializeField] private GameObject _spawnPrefab;
    [SerializeField] private int _spawnCount = 4;
    [SerializeField] private float _spawnRadius = 1f;
    private Vector2 _minBound;
    private Vector2 _maxBound;
    public void SetBounds(Vector2 min, Vector2 max)
    {
        _minBound = min;
        _maxBound = max;
    }
    public int GetSpawnCount() => _spawnCount;

    public GameObject GetSpawnPrefab() => _spawnPrefab;

    public float GetSpawnRadius() => _spawnRadius;

    public override void SetCurrentRoom(Room room)
    {
        base.SetCurrentRoom(room);

        if (room != null)
            SetBounds(room.GetMinBounds(), room.GetMaxBounds());
    }
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
        Room room = GetCurrentRoom();
        if (room != null)
        {
            SetBounds(room.GetMinBounds(), room.GetMaxBounds());
        }
        for (int i = 0; i < _spawnCount; i++)
        {
            float angle = (360f / _spawnCount) * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * _spawnRadius;
          
            Vector3 spawnPos = center + offset;
            spawnPos = ClampToBounds(spawnPos);
            var minon = PoolManager.Instance.Get<MinionEnemy>("Minion");
            minon.SetCurrentRoom(room);
            minon.transform.position = spawnPos;
        }

        // 자신 제거
        ReturnToPool();
    }

    private Vector3 ClampToBounds(Vector3 pos, float margin = 0.3f)
    {
        float x = Mathf.Clamp(pos.x, _minBounds.x + margin, _maxBounds.x - margin);
        float y = Mathf.Clamp(pos.y, _minBounds.y + margin, _maxBounds.y - margin);
        return new Vector3(x, y, pos.z);
    }
}

