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
            OnFire(this, enemyTransform);
        }

      
    }
    public void OnFire(AttackController attack, Transform enemy)
    {
        Vector2 dir = (enemy.position - attack.AttackPivot.transform.position).normalized;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        for (int i = 0; i < StatHandler.AttackCount; i++)
        {
            float offset;

            if (StatHandler.AttackCount % 2 == 1)
            {
                // 홀수: 가운데 탄환은 정확히 타겟 향함
                int middle = StatHandler.AttackCount / 2;
                offset = (i - middle) * StatHandler.AttackAngle;
            }
            else
            {
                // 짝수: 정확히 가운데는 피해서 발사 (빗겨감)
                offset = (i - (StatHandler.AttackCount - 1) / 2f) * StatHandler.AttackAngle;

                if (Mathf.Approximately(offset, 0f))
                    offset += StatHandler.AttackAngle / 2f;
            }

            Quaternion rotation = Quaternion.Euler(0, 0, baseAngle + offset);

            GameObject go = GameObject.Instantiate(
                attack.ProjectilePrefab,
                attack.AttackPivot.transform.position,
                rotation
            );

            var proj = go.GetComponent<Projectile>();
            proj.Init(attack.StatHandler, enemy, attack.ItemEffectManager.GetProjModules());
        }
    }

    // 기즈모를 통한 범위 체크
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(AttackPivot.transform.position, StatHandler.AttackRange);
    }
}
