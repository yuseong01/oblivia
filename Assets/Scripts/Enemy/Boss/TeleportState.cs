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
    Vector2 min = new Vector2(-8,-4);
    Vector2 max = new Vector2(8, 4);
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

        // �Ʒ��� �������� �ִϸ��̼�
        float elapsed = 0f;
        Vector3 downPos = originalPos + Vector3.down * 0.5f;
        while (elapsed < _moveDuration)
        {
            trans.position = Vector3.Lerp(originalPos, downPos, elapsed / _moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        trans.position = downPos;

        // �����̵� ���
        yield return new WaitForSeconds(_stayDuration);

        BoundsUtil.UpdateBoundsFromRoom(obj, ref min, ref max);

        // ���� ��ġ ���� (x �Ǵ� y�� 0�̸� ���� ó��)
        Vector3 teleportPos;
        int attempts = 0;
        do
        {
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);
            teleportPos = new Vector3(x, y, obj.transform.position.z);
            attempts++;
        }
        while ((Mathf.Abs(teleportPos.x) < 0.5f || Mathf.Abs(teleportPos.y) < 0.5f) && attempts < 10);

        trans.position = teleportPos;

        // ���� �ö���� �ִϸ��̼�
        elapsed = 0f;
        Vector3 upPos = teleportPos + Vector3.up * 0.5f;
        while (elapsed < _moveDuration)
        {
            trans.position = Vector3.Lerp(teleportPos, upPos, elapsed / _moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        trans.position = upPos;

        // ���� ����
        if (obj.GetEnemyType() == EnemyType.Teleport)
            obj.ChangeState(new IdleState<T>());
        else
            obj.ChangeState(new AttackState<T>());

    }
}
