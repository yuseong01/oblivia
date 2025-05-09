using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : BaseEnemy<MoveEnemy>
{
    public void TakeDamage(float amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            // FSM 상태 죽음으로
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
