using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneEnemy : BaseEnemy<CloneEnemy>, IExplodable,ISpawnable
{
    [Header("���� or �ֺ� �� ����")]
    [SerializeField] private float _explosionDamage = 10f;
    [SerializeField] private GameObject _spawnPrefab;

    [Header("�̵� ����")]
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
            // ���� ���
            Debug.Log("���� �߻�!");
            // ���⿡ ���� ����Ʈ, ������ ó�� �� �߰� ����
        }

        Destroy(gameObject);
    }
}
