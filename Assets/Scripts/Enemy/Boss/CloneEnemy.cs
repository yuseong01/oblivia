using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneEnemy : BaseEnemy<CloneEnemy>, IExplodable,ISpawnable
{
    [Header("폭발 or 주변 몹 설정")]
    [SerializeField] private float _explosionDamage = 10f;
    [SerializeField] private GameObject _spawnPrefab;

    [Header("이동 설정")]
    [SerializeField] private float _moveSpeed = 1.5f;
    [SerializeField] private float _waitTime = 1f;
    public GameObject GetSpawnPrefab() => _spawnPrefab;
    public int GetSpawnCount() => 3;
    public float GetSpawnRadius() => 2.5f;
    private void Awake()
    {
        if (_player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                _player = playerObj.transform;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Explode();
        }
    }
    public void Explode()
    {
        float chance = Random.value; // 0.0 ~ 1.0

        if (chance < 0.7f)
        {
            // 폭발 방식
            Debug.Log("폭발 발생!");
            // 여기에 폭발 이펙트, 데미지 처리 등 추가 가능
        }

        Destroy(gameObject);
    }
}
