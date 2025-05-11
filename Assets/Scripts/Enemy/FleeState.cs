using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static IEnemy;
using static UnityEditor.PlayerSettings;

public class FleeState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _fleeDistance = 4f; // 플레이어가 이 거리보다 가까우면 도망 시작
    private float _moveSpeed = 3f;
    private float _minFleeDistance = 0.2f; // 의미 있게 도망쳤다고 간주할 최소 거리
    private Vector2 _minBounds = new Vector2(-8, -4); // 맵 최소 좌표
    private Vector2 _maxBounds = new Vector2(8, 4); // 맵 최대 좌표
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

        // 충분히 멀어졌으면 IdleState로 돌아감
        if (Vector2.Distance(enemyPos, playerPos) > _fleeDistance + 0.5f)
        {
            obj.ChangeState(new IdleState<T>());
            return;
        }

        // 목표에 도착했는지 확인
        if (!_hasTarget || Vector2.Distance(enemyPos, _targetPos) < 0.2f)
        {
            SetFleeTarget(obj, enemyPos, playerPos); // 새 도망 위치로 설정
        }

        Vector2 direction = (_targetPos - enemyPos).normalized;
        Vector2 newPos = enemyPos + direction * _moveSpeed * Time.deltaTime;

        // 목표 위치가 현재 위치보다 너무 가까우면 움직이지 말고 거리가 충분하면 이동
        // 필요한 이유는 떨림 방지
        if (Vector2.Distance(enemyPos, _targetPos) > 0.01f)
        {
            obj.GetEnemyPosition().position = newPos;
        }

        // 디버그 시각화
        Debug.DrawLine(enemyPos, _targetPos, Color.red);
        if (obj is Boss && _elapsedTime >= _fleeDuration)
        {
            // 돌진 재진입
            obj.ChangeState(new AttackState<T>()); // 다시 Rush로!
        }
    }

    public void Exit(T obj)
    {
        _hasTarget = false;
    }

    // 도망 목표 위치를 지정하는 함수
    private void SetFleeTarget(T  enemy, Vector2 enemyPos, Vector2 playerPos)
    {
        Vector2 fleeDir = (enemyPos - playerPos).normalized; // 도망 방향

        // 여러 방향 시도
        for (int i = 0; i < 10; i++)
        {
            float angle = Random.Range(-90f, 90f); // fleeDir을 기준으로 ±90도 회전
            Vector2 dir = Quaternion.Euler(0, 0, angle) * fleeDir;
            Vector2 destination = enemyPos + dir * _fleeDistance; // 도망 목표 위치

            // 목표가 유효하면 해당 위치로 설정
            if (IsValidTarget(destination, enemyPos, playerPos))
            {
                _targetPos = ClampToBounds(destination); // 맵 경계 안으로 제한
                _hasTarget = true;
                return;
            }
        }
        // 후보 도망 방향이 전부 막혔을 경우
        // 플레이어 반대 + 맵 중심 쪽 방향을 혼합하여 fallback 방향 계산
        Vector2 fallback = enemyPos + ((fleeDir * 0.6f) + (-enemyPos.normalized) * 0.4f).normalized * (_fleeDistance * 0.5f);
        _targetPos = ClampToBounds(fallback);
        _hasTarget = true;
    }

    // 목표 지점이 유효한지 판단
    private bool IsValidTarget(Vector2 target, Vector2 from, Vector2 player)
    {
        // 맵안에 있는 좌표가 아니면 return
        if (!IsInsideMap(target)) return false;
        float currDist = Vector2.Distance(from, player);
        float nextDist = Vector2.Distance(target, player);
        return nextDist > currDist + _minFleeDistance;
    }
    // 맵안에 있는 좌표인지 확인
    private bool IsInsideMap(Vector2 pos)
    {
        return pos.x >= _minBounds.x && pos.x <= _maxBounds.x &&
               pos.y >= _minBounds.y && pos.y <= _maxBounds.y;
    }
    // 목표 위치가 바깥에 있을 경우 강제로 맵 안으로 끌어오는 함수 
    // 하지만 margin의 여유만큼 안쪽으로
    private Vector2 ClampToBounds(Vector2 pos, float margin = 0.3f)
    {
        float x = Mathf.Clamp(pos.x, _minBounds.x + margin, _maxBounds.x - margin);
        float y = Mathf.Clamp(pos.y, _minBounds.y + margin, _maxBounds.y - margin);
        return new Vector2(x, y);
    }
}
