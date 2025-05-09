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
            // FSM ���� �������� Ȥ�� ���� ���� �߰�
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
