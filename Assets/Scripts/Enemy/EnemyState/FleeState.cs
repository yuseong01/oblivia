using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _fleeDistance = 4f;
    private float _moveSpeed = 5f;
    private float _minFleeDistance = 0.2f;
    private Vector2 _minBounds = new Vector2(-8, -4);
    private Vector2 _maxBounds = new Vector2(8, 4);
    private float _fleeDuration = 2f;
    private float _elapsedTime = 0f;
    private Vector2 _targetPos;
    private bool _hasTarget = false;
    public void SetBounds(Vector2 min, Vector2 max)
    {
        _minBounds = min;
        _maxBounds = max;
    }
    public void Enter(T obj)
    {
        _elapsedTime = 0f;
    }
    public void Update(T obj)
    {
        _elapsedTime += Time.deltaTime;
        Vector2 enemyPos = obj.GetEnemyPosition().position;
        Vector2 playerPos = obj.GetPlayerPosition().position;
        var room = obj.GetCurrentRoom();

        if (room != null)
        {
            SetBounds(room.GetMinBounds(), room.GetMaxBounds());
        }

        if (Vector2.Distance(enemyPos, playerPos) > _fleeDistance + 0.5f)
        {
            obj.ChangeState(new IdleState<T>());
            return;
        }


        if (!_hasTarget || Vector2.Distance(enemyPos, _targetPos) < 0.2f)
        {
            SetFleeTarget(obj, enemyPos, playerPos);
        }

        Vector2 direction = (_targetPos - enemyPos).normalized;
        Vector2 newPos = enemyPos + direction * _moveSpeed * Time.deltaTime;
        Vector2 dir = (_targetPos - enemyPos).normalized;

        if (dir.x != 0)
        {
            Vector3 scale = obj.transform.localScale;
            scale.x = dir.x > 0 ? -1f : 1f; // 플레이어로부터 멀어지는 방향을 바라보게
            obj.transform.localScale = scale;
        }

        if (Vector2.Distance(enemyPos, _targetPos) > 0.01f)
        {
            obj.GetEnemyPosition().position = newPos;
        }

        Debug.DrawLine(enemyPos, _targetPos, Color.red);
        if (!(obj is FleeEnemy) && _elapsedTime >= _fleeDuration)
        {
            obj.ChangeState(new AttackState<T>());
        }
    }

    public void Exit(T obj)
    {
        _hasTarget = false;
    }

    private void SetFleeTarget(T enemy, Vector2 enemyPos, Vector2 playerPos)
    {
        Vector2 fleeDir = (enemyPos - playerPos).normalized;

        for (int i = 0; i < 10; i++)
        {
            float angle = Random.Range(0f, 360f);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * fleeDir;
            Vector2 destination = enemyPos + dir * _fleeDistance;

            if (IsValidTarget(destination, enemyPos, playerPos))
            {
                _targetPos = BoundsUtil.ClampToBounds(destination, _minBounds, _maxBounds);
                _hasTarget = true;
                return;
            }
        }

        Vector2 fallback = enemyPos + ((fleeDir * 0.6f) + (-enemyPos.normalized) * 0.4f).normalized * (_fleeDistance * 0.5f);
        _targetPos = BoundsUtil.ClampToBounds(fallback, _minBounds, _maxBounds);
        _hasTarget = true;
    }

    private bool IsValidTarget(Vector2 target, Vector2 from, Vector2 player)
    {
        if (!BoundsUtil.IsInsideBounds(target, _minBounds, _maxBounds)) return false;
        float currDist = Vector2.Distance(from, player);
        float nextDist = Vector2.Distance(target, player);
        return nextDist > currDist + _minFleeDistance;
    }
}
