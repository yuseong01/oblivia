using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IEnemy;

public class TeleportEnemy : BaseEnemy<TeleportEnemy>
{
    protected override void Awake()
    {
        _type = EnemyType.Teleport;
        base.Awake();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
