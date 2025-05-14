using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable
{
    GameObject GetSpawnPrefab();  // � �������� ��������
    int GetSpawnCount();          // �� �� ��������
    float GetSpawnRadius();       // �󸶸�ŭ ������ ��������
}
/*
 * [SerializeField] private GameObject _spawnPrefab;
    [SerializeField] private int _spawnCount = 4;
    [SerializeField] private float _spawnRadius = 1.5f;
    public int GetSpawnCount() => _spawnCount;

    public GameObject GetSpawnPrefab() => _spawnPrefab;

    public float GetSpawnRadius() => _spawnRadius;
 */