using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObstacle : MonoBehaviour,IObstacle
{
    [SerializeField] protected ObstacleType _type;

    public ObstacleType GetObstacleType() => _type;

    public virtual void OnInteract(GameObject interact)
    {
        // �⺻ ���� �߰�, �ܿ��� �ڽĿ��� ����
    }

}
