using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable
{
    GameObject GetSpawnPrefab();  // � �������� ��������
    int GetSpawnCount();          // �� �� ��������
    float GetSpawnRadius();       // �󸶸�ŭ ������ ��������
}
