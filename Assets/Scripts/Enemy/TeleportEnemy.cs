using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnemy : BaseEnemy<TeleportEnemy>
{
 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
