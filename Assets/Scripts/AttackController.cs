using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // 공격 프리팹
    [SerializeField] private GameObject _projectilePrefab;

    // 공격 주기 관련
    [SerializeField] private float _attackRate = 0.3f;
    [SerializeField] private float _nextAttackTime = 0f;

    // 공격 범위
    [SerializeField] private float _attackRange = 1.0f;
    private Color _gizmoColor = Color.red;

    private void Update()
    {
        if (Time.time >= _nextAttackTime)
        {
            _nextAttackTime = Time.time + _attackRate;

            int enemyLayerMask = LayerMask.GetMask("Enemy");
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, _attackRange, enemyLayerMask);

            if (enemiesInRange.Length > 0)
                FireProjectile(enemiesInRange[0].transform);
        }
    }

    private void FireProjectile(Transform enemyTransform)
    {
        GameObject go = Instantiate(_projectilePrefab, transform.position, transform.rotation);
        var projectile = go.GetComponent<Projectile>();

        //List<IProjectileModule> modules = itemEffectManager.GetModules();
        List<IProjectileModule> modules = new List<IProjectileModule>();

        Vector2 dir = (enemyTransform.position - transform.position).normalized;

        projectile.Init(
        damage: 10f,
        speed: 10f,
        modules: modules,
        direction : dir
    );
    }


    // 기즈모를 통한 범위 체크
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
