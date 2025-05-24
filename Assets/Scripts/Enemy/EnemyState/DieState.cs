using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static IEnemy;

public class DieState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>, IPoolable
{
    private float _timer;
    private float _dieTime = 1f;
    private string _poolKey;
    private EnemyType _type;
    private bool _hasReturned = false;
    public DieState(string poolKey, EnemyType type)
    {
        _poolKey = poolKey;
        _type = type;
    }

    public void Enter(T obj)
    {
        obj.GetAnimator()?.CrossFade("Die", 0f);
    }       

    public void Update(T obj)
    {
        if (_hasReturned) return;

        if (AnimationIsOver(obj))
        {
            _hasReturned = true;
            ReturnToPool(obj);
        }
    }
    private bool AnimationIsOver(T obj)
    {
        Animator anim = obj.GetAnimator();
        if (anim == null) return true;

        return anim.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
    }
    public void Exit(T obj) 
    {

    }

    // 죽으면 몬스터 spawn되는 건지 체크
    private void ReturnToPool(T obj)
    {
        switch (_type)
        {
            case EnemyType.Flee:
                PoolManager.Instance.Return(_poolKey, obj as FleeEnemy);
                break;
            case EnemyType.Normal:
                PoolManager.Instance.Return(_poolKey, obj as MoveEnemy);
                break;
            case EnemyType.Boss:
                PoolManager.Instance.Return(_poolKey, obj as Boss);
                break;
            case EnemyType.Teleport:
                PoolManager.Instance.Return(_poolKey, obj as TeleportEnemy);
                break;
            case EnemyType.Ranged:
                PoolManager.Instance.Return(_poolKey, obj as RangedEnemy);
                break;
            case EnemyType.Rush1:
            case EnemyType.Rush2:
                PoolManager.Instance.Return(_poolKey, obj as RushEnemy);
                break;
            case EnemyType.Minion:
                PoolManager.Instance.Return(_poolKey, obj as MinionEnemy);
                break;
            case EnemyType.Explode:
                PoolManager.Instance.Return(_poolKey, obj as ExplodeEnemy);
                break;
            case EnemyType.Elite1:
            case EnemyType.Elite2:
                PoolManager.Instance.Return(_poolKey, obj as ElitEnemy);
                break;
            default:
                Debug.LogWarning($"[DieState] Unknown enemy type: {_type}");
                break;
        }
    }
}
