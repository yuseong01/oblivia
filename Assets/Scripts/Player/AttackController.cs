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


    // 공격 주기를 위한 변수
    [SerializeField] private float _nextAttackTime = 0f;
    // 공격 범위 색
    private Color _gizmoColor = Color.red;
    // 패밀리어 체크, 추후 길어지면 별도 스크립트로 분리 후 상속
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
   
    // 기즈모를 통한 범위 체크
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        if (StatHandler != null )
            Gizmos.DrawWireSphere(AttackPivot.transform.position, StatHandler.AttackRange);
    }
}
