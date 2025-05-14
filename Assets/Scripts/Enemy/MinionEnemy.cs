using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionEnemy : BaseEnemy<MinionEnemy>
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
}
