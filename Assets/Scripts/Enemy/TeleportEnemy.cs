using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnemy : BaseEnemy<TeleportEnemy>
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 무기와 관련한 태그
        if (other.CompareTag("PlayerBullet"))
        {
            Physics2D.IgnoreCollision(_innerCollider, other, true);
            // 무기 데미지 가져와서 넣으면 될 것 같아요.
            TakeDamage(1); // IEnemy의 구현 메서드
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
