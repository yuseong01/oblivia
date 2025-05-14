using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeEnemy : BaseEnemy<FleeEnemy>
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� ����� ������ �±�
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(1); // IEnemy�� ���� �޼���
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }

}
