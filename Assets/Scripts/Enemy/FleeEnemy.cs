using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeEnemy : BaseEnemy<FleeEnemy>
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerStatHandler playerStatHandler = other.GetComponent<PlayerStatHandler>();
            if (playerStatHandler != null)
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
