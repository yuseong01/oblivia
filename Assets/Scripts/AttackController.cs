using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
     public ItemEffectManager ItemEffectManager;

    // 공격 프리팹
    public GameObject ProjectilePrefab;
    public GameObject AttackPivot;

    // 공격 주기 관련
    [SerializeField] private float _attackRate = 0.3f;
    public float ProjSpeed = 5f;
    [SerializeField] private float _nextAttackTime = 0f;

    // 공격 범위
    [SerializeField] private float _attackRange = 1.0f;
    private Color _gizmoColor = Color.red;

    private void Awake()
    {
        ItemEffectManager = GetComponent<ItemEffectManager>();
    }

    private void Update()
    {
        if (Time.time >= _nextAttackTime)
        {
            _nextAttackTime = Time.time + _attackRate;

            int enemyLayerMask = LayerMask.GetMask("Enemy");
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(AttackPivot.transform.position, _attackRange, enemyLayerMask);

            if (enemiesInRange.Length > 0)
                FireProjectile(enemiesInRange[0].transform);
        }
    }

    private void FireProjectile(Transform enemyTransform)
    {

        var fireMods = ItemEffectManager.GetFireModules();

        if(fireMods.Count > 0)
        {
            foreach (var mod in fireMods)
                mod.OnFire(this, enemyTransform);
        }
        else
        {
            GameObject go = Instantiate(ProjectilePrefab, AttackPivot.transform.position, AttackPivot.transform.rotation);
            var projectile = go.GetComponent<Projectile>();

            Vector2 dir = (enemyTransform.position - transform.position).normalized;
            go.transform.up = dir;

            projectile.Init(
            damage: 10f,
            speed: ProjSpeed,
            modules: ItemEffectManager.GetProjModules(),
            enemyTransform: enemyTransform
        );
        }

      
    }


    // 기즈모를 통한 범위 체크
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(AttackPivot.transform.position, _attackRange);
    }
}
