using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static IEnemy;

public class DieState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>, IPoolable
{
    private float _timer;
    private float _dieTime = 2f;
    private string _poolKey;
    private BaseEnemy<T> _baseEnemy;

    public DieState(string poolKey)
    {
        _poolKey = poolKey;
    }

    public void Enter(T obj)
    {
        _baseEnemy = obj.GetComponent<BaseEnemy<T>>();
        obj.GetAnimator()?.CrossFade("Die", 0f);
    }       

    public void ReturnPool()
    {
       // _baseEnemy?.ReturnToPool();
    }

    public void Update(T obj)
    {
        _timer += Time.deltaTime;
        if (_timer >= _dieTime)
        {
            _baseEnemy?.ReturnToPool();
        }
    }
   public void Exit(T obj) { }

   // 죽으면 몬스터 spawn되는 건지 체크
  
}
