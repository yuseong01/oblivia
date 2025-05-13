using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IEnemy;

public class RangedEnemy : BaseEnemy<RangedEnemy>, IRangedEnemy
{
    [Header("Projectile")]
    [SerializeField] private GameObject forwardProjectile;

    private void Start()
    {
        _type = EnemyType.Ranged;
    }

    public GameObject GetProjectilePrefab(string type)
    {
        return forwardProjectile;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� ����� ������ �±�
        if (other.CompareTag("PlayerBullet"))
        {
            PlayerStatHandler playerStatHandler = other.GetComponent<PlayerStatHandler>();
            if (playerStatHandler != null)
            {
                playerStatHandler.Health = -GetAttackPower();
            }
            Destroy(other.gameObject); // bullet�� ����
        }
    }
}
