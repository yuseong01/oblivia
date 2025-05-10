using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class AttackController : ProjectileShooterBase
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _attackPivot;
    [SerializeField] private PlayerStatHandler _statHandler;
    [SerializeField] private ItemEffectManager _itemEffectManager;

    public override GameObject ProjectilePrefab
    {
        get => _projectilePrefab;
        set => _projectilePrefab = value;
    }

    public override GameObject AttackPivot
    {
        get => _attackPivot;
        set => _attackPivot = value;
    }

    public override PlayerStatHandler StatHandler
    {
        get => _statHandler;
        set => _statHandler = value;
    }

    public override ItemEffectManager ItemEffectManager
    {
        get => _itemEffectManager;
        set => _itemEffectManager = value;
    }


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
            _itemEffectManager = GetComponent<ItemEffectManager>();
            if (StatHandler == null)
                _statHandler = GetComponent<PlayerStatHandler>();
        }
    }

    private void Update()
    {
        if (Time.time >= _nextAttackTime)
        {
            if (StatHandler == null || ItemEffectManager == null)
                return;

            _nextAttackTime = Time.time + StatHandler.AttackRate;

            Collider2D[] enemiesInRange = CheckEnemy();

            if (enemiesInRange.Length > 0)
                FireProjectile(enemiesInRange[0].transform);
        }
    }
    public Collider2D[] CheckEnemy()
    {
        int enemyLayerMask = LayerMask.GetMask("Enemy");
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(AttackPivot.transform.position, StatHandler.AttackRange, enemyLayerMask);
        return enemiesInRange;
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
            OnFire( enemyTransform);
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
