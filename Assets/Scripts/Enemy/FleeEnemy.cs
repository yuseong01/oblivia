using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeEnemy : BaseEnemy<FleeEnemy>
{
    public void TakeDamage(float amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            // FSM 상태 죽음으로 혹은 죽음 로직 추가
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
