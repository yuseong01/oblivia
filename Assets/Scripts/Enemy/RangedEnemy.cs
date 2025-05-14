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
        // 플레이어와 무기와 관련한 태그
        if (other.CompareTag("PlayerBullet"))
        {
            Physics2D.IgnoreCollision(_innerCollider, other, true);
            PlayerStatHandler playerStatHandler = other.GetComponent<PlayerStatHandler>();
            if (playerStatHandler != null)
            {
                playerStatHandler.Health = -GetAttackPower();
            }
        }
    }
}
