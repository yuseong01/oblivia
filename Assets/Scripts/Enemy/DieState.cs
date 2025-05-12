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
            if (chance <= 1f) // Ȯ�� ���� ���ϸ� ����
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
            // RoomManager.Instance?.EnemyDied(); ���� �̷��� Enemy�� �׾��ٴ� ���� RoomManager�� �˸��� �� �� ���ƿ�
            // ������ Enemy�� ������ ���� �����Բ�? �Ʒ� �ڵ� ����
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
