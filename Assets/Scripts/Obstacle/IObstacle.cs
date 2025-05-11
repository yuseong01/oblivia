using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ObstacleType
{
    Pushable,
    Breakable,
    Static
}
public interface IObstacle
{
    ObstacleType GetObstacleType();
    void OnInteract(GameObject interactor); // 밀거나 공격 받을 때 호출
}

/* 플레이어와 상호작용할때
 private void InteractObstacle(Collider other)
{
    IObstacle obs = other.GetComponent<IObstacle>();
    if (obs != null)
    {
        obs.OnInteract(this.gameObject); // player = interactor
    }
}
 */