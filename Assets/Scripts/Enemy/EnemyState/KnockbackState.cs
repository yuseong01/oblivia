using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _pauseTime = 0.1f;
    private float _timer;
    private Vector2 _knockbackDir;
    private float _knockbackPower = 15f;
    private float _drag = 20f;
    private Vector2 _velocity;
    private Vector2 _minBounds = new Vector2(-8, -4); // ?? ??? ???
    private Vector2 _maxBounds = new Vector2(8, 4); // ?? ??? ???
    public void SetBounds(Vector2 min, Vector2 max)
    {
        _minBounds = min;
        _maxBounds = max;
    }
    public void Enter(T obj)
    {
        _timer = 0f;
        obj.GetAnimator()?.CrossFade("Idle", 0f);

        Transform enemyTransform = obj.GetEnemyPosition();
        Transform playerTransform = obj.GetPlayerPosition();

        if (enemyTransform != null && playerTransform != null)
        {
            _knockbackDir = (enemyTransform.position - playerTransform.position).normalized;
            _velocity = _knockbackDir * _knockbackPower;
        }
        else
        {
            _velocity = Vector2.zero;
        }

    }

    public void Update(T obj)
    {
        _timer += Time.deltaTime;
        _velocity = Vector2.Lerp(_velocity, Vector2.zero, _drag * Time.deltaTime);

        // 현재 룸 기준으로 바운드 설정
        var room = obj.GetCurrentRoom();
        if (room != null)
        {
            SetBounds(room.GetMinBounds(), room.GetMaxBounds());
        }

        Vector2 newPos = (Vector2)obj.transform.position + _velocity * Time.deltaTime;
        newPos = ClampToBounds(newPos);
        obj.transform.position = newPos;

        if (_timer >= _pauseTime)
        {
            obj.ChangeState(new IdleState<T>());
        }
    }
    private bool IsInsideMap(Vector2 pos)
    {
        return pos.x >= _minBounds.x && pos.x <= _maxBounds.x && pos.y >= _minBounds.y && pos.y <= _maxBounds.y;
    }
    // ??? ????? ????? ???? ??? ?????? ?? ?????? ??????? ??? 
    // ?????? margin?? ??????? ????????
    private Vector2 ClampToBounds(Vector2 pos, float margin = 0.3f)
    {
        float x = Mathf.Clamp(pos.x, _minBounds.x + margin, _maxBounds.x - margin);
        float y = Mathf.Clamp(pos.y, _minBounds.y + margin, _maxBounds.y - margin);
        return new Vector2(x, y);
    }
    public void Exit(T obj)
    {
    }
}
