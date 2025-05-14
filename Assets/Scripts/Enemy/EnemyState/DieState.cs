using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static IEnemy;

public class DieState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>, IPoolable
{
    private float _timer;
    private float _dieTime = 1f;
    private string _poolKey;
    public DieState(string poolKey)
    {
        _poolKey = poolKey;
    }


    public void Enter(T obj)
    {
        _timer = 0f;
        //obj.GetAnimator()?.SetTrigger("Die");
        // obj�� �����Ǵ� �ָ� �׾����� ������Ű�� 
        CheckRadius(obj);
        (obj as BaseEnemy<T>)?.ReturnToPool();
    }

    public void Update(T obj)
    {
       
    }
   public void Exit(T obj) { }

   // ������ ���� spawn�Ǵ� ���� üũ
   public void CheckRadius(T obj)
   {
      if (obj is ISpawnable spawnable)
      {
          GameObject prefab = spawnable.GetSpawnPrefab();
          int count = spawnable.GetSpawnCount();
          float radius = spawnable.GetSpawnRadius();
          for (int i = 0; i < count; i++)
          {
              // 360�� ���� �������� ��ġ�� ���� ���
              float angle = (360f / count) * i * Mathf.Deg2Rad;
              // ���� ���� ���� cos : x/ sin : y
              Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
              Vector3 spawnPos = obj.transform.position + dir * radius;
              GameObject.Instantiate(prefab, spawnPos, Quaternion.identity);        
            }
      }
  }
}
