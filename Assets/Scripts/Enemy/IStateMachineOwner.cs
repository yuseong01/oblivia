using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachineOwner<T>
{ 
    void ChangeState(IState<T> newState);
}
