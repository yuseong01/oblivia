using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static IEnemy;
using static UnityEditor.PlayerSettings;

public class FleeState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _fleeDistance = 4f; // �÷��̾ �� �Ÿ����� ������ ���� ����
    private float _moveSpeed = 3f;
    private float _minFleeDistance = 0.2f; // �ǹ� �ְ� �����ƴٰ� ������ �ּ� �Ÿ�
    private Vector2 _minBounds = new Vector2(-8, -4); // �� �ּ� ��ǥ
    private Vector2 _maxBounds = new Vector2(8, 4); // �� �ִ� ��ǥ
    private float _fleeDuration = 2f;
    private float _elapsedTime = 0f;
    private Vector2 _targetPos;
    private bool _hasTarget = false;

    public void Enter(T obj)
    {
        _elapsedTime = 0f;
    }

    public void Update(T obj)
    {
        _elapsedTime += Time.deltaTime;
        Vector2 enemyPos = obj.GetEnemyPosition().position;
        Vector2 playerPos = obj.GetPlayerPosition().position;

        // ����� �־������� IdleState�� ���ư�
        if (Vector2.Distance(enemyPos, playerPos) > _fleeDistance + 0.5f)
        {
            obj.ChangeState(new IdleState<T>());
            return;
        }

        // ��ǥ�� �����ߴ��� Ȯ��
        if (!_hasTarget || Vector2.Distance(enemyPos, _targetPos) < 0.2f)
        {
            SetFleeTarget(obj, enemyPos, playerPos); // �� ���� ��ġ�� ����
        }

        Vector2 direction = (_targetPos - enemyPos).normalized;
        Vector2 newPos = enemyPos + direction * _moveSpeed * Time.deltaTime;

        // ��ǥ ��ġ�� ���� ��ġ���� �ʹ� ������ �������� ���� �Ÿ��� ����ϸ� �̵�
        // �ʿ��� ������ ���� ����
        if (Vector2.Distance(enemyPos, _targetPos) > 0.01f)
        {
            obj.GetEnemyPosition().position = newPos;
        }

        // ����� �ð�ȭ
        Debug.DrawLine(enemyPos, _targetPos, Color.red);
        if (obj is Boss && _elapsedTime >= _fleeDuration)
        {
            // ���� ������
            obj.ChangeState(new AttackState<T>()); // �ٽ� Rush��!
        }
    }

    public void Exit(T obj)
    {
        _hasTarget = false;
    }

    // ���� ��ǥ ��ġ�� �����ϴ� �Լ�
    private void SetFleeTarget(T  enemy, Vector2 enemyPos, Vector2 playerPos)
    {
        Vector2 fleeDir = (enemyPos - playerPos).normalized; // ���� ����

        // ���� ���� �õ�
        for (int i = 0; i < 10; i++)
        {
            float angle = Random.Range(-90f, 90f); // fleeDir�� �������� ��90�� ȸ��
            Vector2 dir = Quaternion.Euler(0, 0, angle) * fleeDir;
            Vector2 destination = enemyPos + dir * _fleeDistance; // ���� ��ǥ ��ġ

            // ��ǥ�� ��ȿ�ϸ� �ش� ��ġ�� ����
            if (IsValidTarget(destination, enemyPos, playerPos))
            {
                _targetPos = ClampToBounds(destination); // �� ��� ������ ����
                _hasTarget = true;
                return;
            }
        }
        // �ĺ� ���� ������ ���� ������ ���
        // �÷��̾� �ݴ� + �� �߽� �� ������ ȥ���Ͽ� fallback ���� ���
        Vector2 fallback = enemyPos + ((fleeDir * 0.6f) + (-enemyPos.normalized) * 0.4f).normalized * (_fleeDistance * 0.5f);
        _targetPos = ClampToBounds(fallback);
        _hasTarget = true;
    }

    // ��ǥ ������ ��ȿ���� �Ǵ�
    private bool IsValidTarget(Vector2 target, Vector2 from, Vector2 player)
    {
        // �ʾȿ� �ִ� ��ǥ�� �ƴϸ� return
        if (!IsInsideMap(target)) return false;
        float currDist = Vector2.Distance(from, player);
        float nextDist = Vector2.Distance(target, player);
        return nextDist > currDist + _minFleeDistance;
    }
    // �ʾȿ� �ִ� ��ǥ���� Ȯ��
    private bool IsInsideMap(Vector2 pos)
    {
        return pos.x >= _minBounds.x && pos.x <= _maxBounds.x &&
               pos.y >= _minBounds.y && pos.y <= _maxBounds.y;
    }
    // ��ǥ ��ġ�� �ٱ��� ���� ��� ������ �� ������ ������� �Լ� 
    // ������ margin�� ������ŭ ��������
    private Vector2 ClampToBounds(Vector2 pos, float margin = 0.3f)
    {
        float x = Mathf.Clamp(pos.x, _minBounds.x + margin, _maxBounds.x - margin);
        float y = Mathf.Clamp(pos.y, _minBounds.y + margin, _maxBounds.y - margin);
        return new Vector2(x, y);
    }
}
