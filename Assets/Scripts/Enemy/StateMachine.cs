using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private IState<T> _cuurentState;

    // state�ٲ� ������ ������ ���ο�� enter
    public void ChangeState(IState<T> newState, T obj)
    {
        _cuurentState?.Exit(obj);
        _cuurentState = newState;
        _cuurentState?.Enter(obj);
    }

    public void Update(T obj)
    {
        _cuurentState?.Update(obj);
    }
}
