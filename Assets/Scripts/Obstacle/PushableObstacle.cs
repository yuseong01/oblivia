using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObstacle : BaseObstacle
{
    [SerializeField] private float _pushForce = 5f;
    private Rigidbody _rb;

    private void Awake()
    {
        _type = ObstacleType.Pushable;
        _rb = GetComponent<Rigidbody>();
    }

    public override void OnInteract(GameObject interact)
    {
        base.OnInteract(interact);
        Vector3 pushDir = (transform.position - interact.transform.position).normalized;
        _rb.AddForce(pushDir * _pushForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Obstacle"))
        {
            // ���߱�
            _rb.velocity = Vector2.zero;
        }

        if (collision.CompareTag("Player"))
        {
            // �ʿ�� ���س� ���� ��
        }
    }
}
