using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObstacle : BaseObstacle
{
    [SerializeField] private GameObject dropItem;
    [SerializeField] private int health = 1; // 체력 설정

    private void Awake()
    {
        _type = ObstacleType.Breakable;
    }

    public override void OnInteract(GameObject interactor)
    {
        health--;
        if (health <= 0)
        {
            if (dropItem != null)
                Instantiate(dropItem, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
