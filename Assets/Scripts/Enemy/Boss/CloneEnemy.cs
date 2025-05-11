using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneEnemy : BaseEnemy<CloneEnemy>, IExplodable,ISpawnable
{
    [Header("폭발 or 주변 몹 설정")]
    [SerializeField] private float _explosionDamage = 10f;
    [SerializeField] private GameObject _spawnPrefab;
    [Header("맵 이동 경계")]
    [SerializeField] private Vector2 _minBounds = new Vector2(-8f, -4f); // 맵 최소 좌표 (x, z)
    [SerializeField] private Vector2 _maxBounds = new Vector2(8f, 4f);   // 맵 최대 좌표 (x, z)

    [Header("이동 설정")]
    [SerializeField] private float _moveSpeed = 1.5f;
    [SerializeField] private float _waitTime = 1f;

    private Coroutine _moveRoutine;
    private void Awake()
    {
        if (_player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                _player = playerObj.transform;
        }
    }
    private void OnTriggerEnter(Collider other)
    { 
        Debug.Log("OnTriggerEnter 발생! → " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("다음");
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

    public GameObject GetSpawnPrefab() => _spawnPrefab;
    public int GetSpawnCount() => 3;
    public float GetSpawnRadius() => 2.5f;
}
