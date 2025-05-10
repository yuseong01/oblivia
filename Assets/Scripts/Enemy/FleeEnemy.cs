using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeEnemy : BaseEnemy<FleeEnemy>
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 무기와 관련한 태그
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(1); // IEnemy의 구현 메서드
            Destroy(other.gameObject); // bullet도 제거
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
