using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IEnemy;

public class RangedEnemy : BaseEnemy<RangedEnemy>, IRangedEnemy
{
    [Header("Projectile")]
    [SerializeField] private GameObject forwardProjectile;

    protected override void Start()
    {
        base.Start();
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
            TakeDamage(1); // IEnemy�� ���� �޼���
            Destroy(other.gameObject); // bullet�� ����
        }
    }
}
