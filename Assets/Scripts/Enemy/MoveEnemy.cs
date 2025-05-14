using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : BaseEnemy<MoveEnemy>
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(_innerCollider, other, true);
            PlayerStatHandler playerStatHandler = other.GetComponent<PlayerStatHandler>();
            if(playerStatHandler != null)
            {
                playerStatHandler.Health = -GetAttackPower();
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
