using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : BaseEnemy<MoveEnemy>
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� ����� ������ �±�
        if (other.CompareTag("PlayerBullet"))
        {
            // ���� ������ �����ͼ� ������ �� �� ���ƿ�.
            TakeDamage(1); // IEnemy�� ���� �޼���
            Destroy(other.gameObject); // bullet�� ����
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
