using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _timer;
    private float _dieTime = 1f;

    public void Enter(T obj)
    {
        _timer = 0f;
        //obj.GetAnimator()?.SetTrigger("Die");

        var col = obj.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

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
                    GameObject.Instantiate(prefab, pos, Quaternion.identity);
                }
            }
        }
    }

    public void Update(T obj)
    {
        _timer += Time.deltaTime;

        if (_timer >= _dieTime)
        {
            // RoomManager.Instance?.EnemyDied(); 뭔가 이렇게 Enemy가 죽었다는 것을 RoomManager에 알리면 될 것 같아요
            // 마지막 Enemy가 죽으면 문이 열리게끔? 아래 코드 느낌
            /*
             * public void OnEnemyDied()
                 {
                     aliveEnemyCount--;
        

               if (aliveEnemyCount <= 0)
               {
                 HandleRoomClear();
                 }
                  }
             */
            GameObject.Destroy(obj.gameObject);
        }
    }
   public void Exit(T obj) { }
}
