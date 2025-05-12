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
        /*
        if (obj is ISpawnable spawnable)
        {
            float chance = Random.value;
            if (chance <= 1f) // 확률 조절 원하면 변경
            {
                GameObject prefab = spawnable.GetSpawnPrefab();
                int count = spawnable.GetSpawnCount();
                float radius = spawnable.GetSpawnRadius();

                for (int i = 0; i < count; i++)
                {
                    Vector2 offset = Random.insideUnitCircle * radius;
                    Vector3 pos = obj.transform.position + new Vector3(offset.x, 0f, offset.y);
                    //GameObject.Instantiate(prefab, pos, Quaternion.identity);
                }
            }
        }*/
    }

    public void Update(T obj)
    {
        _timer += Time.deltaTime;

        if (_timer >= _dieTime)
        {
            // Room에 알림 (선택적)
            //RoomManager.Instance?.OnEnemyDied();
            // null체크하고 불러오기
            (obj as BaseEnemy<T>)?.ReturnToPool();
        }
    }
   public void Exit(T obj) { }
}
