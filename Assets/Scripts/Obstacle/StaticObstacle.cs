using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObstacle : BaseObstacle
{
    private void Awake()
    {
        _type = ObstacleType.Static;
    }

    public override void OnInteract(GameObject interactor)
    {
        // 아무 동작 없음
    }
}
