using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMosnter : MonoBehaviour
{
    // projectile �����غ����� ���� ��ũ��Ʈ�Դϴ�.
    // ���߿� ���� ��ũ��Ʈ
    private Vector3 _dir;
    private int _damage;

    public void Initialize(Vector3 direction, int damage)
    {
        _dir = direction.normalized;
        _damage = damage;
    }

    private void Update()
    {
        transform.position += _dir * 6f * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatHandler playerStatHandler = other.GetComponent<PlayerStatHandler>();
            if (playerStatHandler != null)
            {
                playerStatHandler.Health = -_damage;
            }
            Destroy(gameObject);
        }
    }
}
