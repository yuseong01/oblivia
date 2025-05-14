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
    void OnInteract(GameObject interactor); // �аų� ���� ���� �� ȣ��
}

/* �÷��̾�� ��ȣ�ۿ��Ҷ�
 private void InteractObstacle(Collider other)
{
    IObstacle obs = other.GetComponent<IObstacle>();
    if (obs != null)
    {
        obs.OnInteract(this.gameObject); // player = interactor
    }
}
 */