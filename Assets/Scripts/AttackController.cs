using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public ItemEffectManager ItemEffectManager;
    public PlayerStatHandler StatHandler;

    // 공격 프리팹
    public GameObject ProjectilePrefab;
    public GameObject AttackPivot;

    // 공격 주기를 위한 변수
    [SerializeField] private float _nextAttackTime = 0f;
    // 공격 범위 색
    private Color _gizmoColor = Color.red;

    private void Awake()
    {
        ItemEffectManager = GetComponent<ItemEffectManager>();
        if(StatHandler == null)
             StatHandler = GetComponent<PlayerStatHandler>();
    }

    private void Update()
    {
        if (Time.time >= _nextAttackTime)
        {
            _nextAttackTime = Time.time + StatHandler.AttackRate;

            int enemyLayerMask = LayerMask.GetMask("Enemy");
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(AttackPivot.transform.position, StatHandler.AttackRange, enemyLayerMask);

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
            statHandler: StatHandler,
            modules: ItemEffectManager.GetProjModules(),
            enemyTransform: enemyTransform
        );
        }

      
    }


    // 기즈모를 통한 범위 체크
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(AttackPivot.transform.position, StatHandler.AttackRange);
    }
}
