using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable
{
    GameObject GetSpawnPrefab();  // 어떤 프리팹을 생성할지
    int GetSpawnCount();          // 몇 개 생성할지
    float GetSpawnRadius();       // 얼마만큼 퍼지게 생성할지
}
/*
 * [SerializeField] private GameObject _spawnPrefab;
    [SerializeField] private int _spawnCount = 4;
    [SerializeField] private float _spawnRadius = 1.5f;
    public int GetSpawnCount() => _spawnCount;

    public GameObject GetSpawnPrefab() => _spawnPrefab;

    public float GetSpawnRadius() => _spawnRadius;
 */