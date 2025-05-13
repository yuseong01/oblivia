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
        var col = obj.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        // obj�� �����Ǵ� �ָ� �׾����� ������Ű�� 
        CheckRadius(obj);
    }

    public void Update(T obj)
    {
        _timer += Time.deltaTime;

        if (_timer >= _dieTime)
        {
            var col = obj.GetComponent<Collider2D>();
            if (col != null) col.enabled = true;
            // Room�� �˸� (������)
            //RoomManager.Instance?.OnEnemyDied();
            // nullüũ�ϰ� �ҷ�����
            (obj as BaseEnemy<T>)?.ReturnToPool();
        }
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
