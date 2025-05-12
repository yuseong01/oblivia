using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _moveDuration = 0.3f;
    private float _stayDuration = 0.2f;
    private Vector2 _minBounds = new Vector2(-8f, -4f);
    private Vector2 _maxBounds = new Vector2(8f, 4f);

    public void Enter(T obj)
    {
        obj.StartCoroutine(TeleportRoutine(obj));
    }

    public void Update(T obj) { }

    public void Exit(T obj) { }

    private IEnumerator TeleportRoutine(T obj)
    {
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
        float x = Random.Range(_minBounds.x, _maxBounds.x);
        float y = Mathf.Clamp(Random.Range(-4f, 4f), _minBounds.y+downPos.y, _maxBounds.y+downPos.y);
        Vector3 teleportPos = new Vector3(x, y, trans.position.z);
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
        obj.ChangeState(new AttackState<T>());
    }
}
