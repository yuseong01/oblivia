using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private float _rushSpeed = 5f;
    private float _rushDuration = 1f;
    private float _elapsedTime = 0f;
    private Vector3 _rushDirection;
    public void Enter(T obj)
    {
        Debug.Log("��������");
        _elapsedTime = 0f;

        // ���� ����: �÷��̾� ����
        Vector3 toPlayer = obj.GetPlayerPosition().position - obj.transform.position;
        _rushDirection = toPlayer.normalized;
       
    }

    public void Update(T obj)
    {
        _elapsedTime += Time.deltaTime;

        /// ���� �̵�
        obj.transform.position += _rushDirection * _rushSpeed * Time.deltaTime;

        // ���� ����
        if (_elapsedTime >= _rushDuration)
        {
            obj.ChangeState(new FleeState<T>());
        }
    }

    public void Exit(T obj)
    {

    }
}
