using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static IEnemy;

public class TeleportState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _moveDuration = 0.3f;
    private float _stayDuration = 0.2f;
    private Coroutine _teleportRoutine;

    public void Enter(T obj)
    {
        obj.StartCoroutine(TeleportRoutine(obj));
    }

    public void Update(T obj) 
    {
    }

    public void Exit(T obj) 
    {
        if (_teleportRoutine != null)
        {
            obj.StopCoroutine(_teleportRoutine);
            _teleportRoutine = null;
        }
    }

    private IEnumerator TeleportRoutine(T obj)
    {
        if (obj.GetHealth() <= 0f) yield break;
        Transform trans = obj.transform;
        Vector3 originalPos = trans.position;

        // 아래로 내려가는 애니메이션
        float elapsed = 0f;
        Vector3 downPos = originalPos + Vector3.down * 0.5f;
        while (elapsed < _moveDuration)
        {
            trans.position = Vector3.Lerp(originalPos, downPos, elapsed / _moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        trans.position = downPos;

        // 순간이동
        yield return new WaitForSeconds(_stayDuration);

        Vector2 min = new Vector2();
        Vector2 max = new Vector2();

        var room = obj.GetCurrentRoom();
        if (room != null)
        {
            min = room.GetMinBounds();
            max = room.GetMaxBounds();
         //   yield return null; ;
        }

        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);
        Vector3 teleportPos = new Vector3(x, y, obj.transform.position.z);

        trans.position = teleportPos;

        // 위로 올라오는 애니메이션
        elapsed = 0f;
        Vector3 upPos = teleportPos + Vector3.up * 0.5f;
        while (elapsed < _moveDuration)
        {
            trans.position = Vector3.Lerp(teleportPos, upPos, elapsed / _moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        trans.position = upPos;

        // 다음 상태
        if(obj.GetEnemyType() == EnemyType.Teleport)
            obj.ChangeState(new IdleState<T>());
        else obj.ChangeState(new AttackState<T>());
        

    }
}
