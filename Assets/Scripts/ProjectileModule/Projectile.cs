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
    [SerializeField] private LayerMask _targetLayers;
    public float HitCooldown = 0.2f; // 공격 인터벌

    private Dictionary<Collider2D, float> _lastHitTime = new Dictionary<Collider2D, float>();

    public void Init(PlayerStatHandler statHandler, Transform enemyTransform, List<IProjectileModule> modules)
    {
        _damage = statHandler.Damage;
        Speed = statHandler.AttackSpeed;
        _modules = modules;
        Target = enemyTransform;

        foreach (var mod in modules)
        {
            mod.OnFire(this);
        }

        //테스트 삭제 추후 제거
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (((1 << collision.gameObject.layer) & _targetLayers) != 0)
        {
            float lastTime;
            _lastHitTime.TryGetValue(collision, out lastTime);

            if (Time.time - lastTime >= HitCooldown)
            {
                // collision.GetComponent<Enemy>()?.TakeDamage(_damage);
                Debug.Log($"적 공격{_damage}");
                _lastHitTime[collision] = Time.time;

                if (!CanPenetrate)
                    Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        transform.position += transform.up * Speed * Time.deltaTime;

        foreach (var mod in _modules)
        {
            mod.OnUpdate(this);
        }
    }

}
