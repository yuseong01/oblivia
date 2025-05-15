using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _rushSpeed = 3f;
    private float _rushDuration = 1f;
    private float _elapsedTime = 0f;
    private Vector3 _rushDirection;

    private Vector2 _minBounds = new Vector2(-8, -4);
    private Vector2 _maxBounds = new Vector2(8, 4);
    public void SetBounds(Vector2 min, Vector2 max)
    {
        _minBounds = min;
        _maxBounds = max;
    }
    public void Enter(T obj)
    {
        _elapsedTime = 0f;
        obj.GetAnimator()?.CrossFade("Attack", 0f);

        // 돌진 방향: 플레이어 향함
        Vector3 toPlayer = obj.GetPlayerPosition().position - obj.transform.position;
        _rushDirection = toPlayer.normalized;

        var sr = (obj as MonoBehaviour).GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = _rushDirection.x < 0;
        }

    }

    public void Update(T obj)
    {
        _elapsedTime += Time.deltaTime;

        /// 돌진 이동
        Vector3 newPos = obj.transform.position + _rushDirection * _rushSpeed * Time.deltaTime;


        var room = obj.GetCurrentRoom();

        if (room != null)
        {
            SetBounds(room.GetMinBounds(), room.GetMaxBounds());
        }


        newPos = ClampToBounds(newPos); // 맵 내인지 체크

        obj.transform.position = newPos;

        // 돌진 종료
        if (_elapsedTime >= _rushDuration)
        {
            obj.ChangeState(new FleeState<T>());
        }
    }

    public void Exit(T obj)
    {

    }

    private Vector3 ClampToBounds(Vector3 pos, float margin = 0.3f)
    {
        float x = Mathf.Clamp(pos.x, _minBounds.x + margin, _maxBounds.x - margin);
        float y = Mathf.Clamp(pos.y, _minBounds.y + margin, _maxBounds.y - margin);
        return new Vector3(x, y, pos.z);
    }
}
