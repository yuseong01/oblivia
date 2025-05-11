using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
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
