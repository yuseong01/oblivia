using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    private float _damage;
    private List<IProjectileModule> _modules;
    private Vector2 _direction;
    public Transform Target;
    public bool CanPenetrate;
    public  float AttackDuration;
    private PlayerStatHandler _statHandler;
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private LayerMask _wallLayers;
    public float HitCooldown = 0.2f;

    private Dictionary<Collider2D, float> _lastHitTime = new Dictionary<Collider2D, float>();

    public void Init(PlayerStatHandler statHandler, Transform enemyTransform, List<IProjectileModule> modules)
    {
        _statHandler = statHandler;
        _damage = statHandler.Damage;
        Speed = statHandler.AttackSpeed;
        _modules = modules;
        Target = enemyTransform;
        AttackDuration = statHandler.AttackDuration;
        this.transform.localScale = new Vector2(statHandler.ProjectileSize, statHandler.ProjectileSize);

        foreach (var mod in modules)
        {
            mod.OnFire(this);
        }

        Destroy(gameObject, AttackDuration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (((1 << collision.gameObject.layer) & _targetLayers) != 0)
        {
            float lastTime;
            _lastHitTime.TryGetValue(collision, out lastTime);

            if (Time.time - lastTime >= HitCooldown)
            {
                var enemy = collision.GetComponent<IEnemy>();
                
                _lastHitTime[collision] = Time.time;

                Rigidbody2D rb = collision.attachedRigidbody;
                Vector2 knockbackDir = Vector2.zero;
                if (rb != null)
                {
                    knockbackDir = transform.up; 
                    rb.AddForce(knockbackDir * _statHandler.KnockbackForce, ForceMode2D.Impulse);
                }
                if (enemy != null)
                {
                    enemy.TakeDamage(_damage, knockbackDir);
                }
                if (!CanPenetrate)
                    Destroy(gameObject);
            }
        }
        if (((1 << collision.gameObject.layer) & _wallLayers) != 0)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(Target == null)
        {
            Destroy(gameObject);
        }
        foreach (var mod in _modules)
        {
            mod.OnUpdate(this);
        }

        transform.position += transform.up * Speed * Time.deltaTime;
    }

}
