using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObstacle : MonoBehaviour,IObstacle
{
    [SerializeField] protected ObstacleType _type;

    public ObstacleType GetObstacleType() => _type;

    public virtual void OnInteract(GameObject interact)
    {
        // 기본 동작 추가, 외에는 자식에서 구현
    }

}
