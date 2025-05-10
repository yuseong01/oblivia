using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public ItemEffectManager ItemEffectManager;
    public PlayerStatHandler StatHandler;

    // ���� ������
    public GameObject ProjectilePrefab;
    public GameObject AttackPivot;

    // ���� �ֱ⸦ ���� ����
    [SerializeField] private float _nextAttackTime = 0f;
    // ���� ���� ��
    private Color _gizmoColor = Color.red;
    // �йи��� üũ, ���� ������� ���� ��ũ��Ʈ�� �и� �� ���
    [SerializeField] bool _isFamiliar;

    private void Awake()
    {
        if (!_isFamiliar)
        {
            ItemEffectManager = GetComponent<ItemEffectManager>();
            if (StatHandler == null)
                StatHandler = GetComponent<PlayerStatHandler>();
        }
        else
        {
     
        }

    }

    private void Update()
    {
        if (Time.time >= _nextAttackTime)
        {
            if (StatHandler == null || ItemEffectManager == null)
                return;

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
                // Ȧ��: ��� źȯ�� ��Ȯ�� Ÿ�� ����
                int middle = StatHandler.AttackCount / 2;
                offset = (i - middle) * StatHandler.AttackAngle;
            }
            else
            {
                // ¦��: ��Ȯ�� ����� ���ؼ� �߻� (���ܰ�)
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

    // ����� ���� ���� üũ
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        if (StatHandler != null )
            Gizmos.DrawWireSphere(AttackPivot.transform.position, StatHandler.AttackRange);
    }
}
