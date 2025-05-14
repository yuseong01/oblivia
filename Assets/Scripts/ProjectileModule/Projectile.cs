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
    public float HitCooldown = 0.2f; // ���� ���͹�

    private Dictionary<Collider2D, float> _lastHitTime = new Dictionary<Collider2D, float>();

    public void Init(PlayerStatHandler statHandler, Transform enemyTransform, List<IProjectileModule> modules)
    {
        _statHandler = statHandler;
        _damage = statHandler.Damage;
        Speed = statHandler.AttackSpeed;
        _modules = modules;
        Target = enemyTransform;
        AttackDuration = statHandler.AttackDuration;
        //źȯ ũ��
        this.transform.localScale = new Vector2(statHandler.ProjectileSize, statHandler.ProjectileSize);

        foreach (var mod in modules)
        {
            mod.OnFire(this);
        }

        //�׽�Ʈ ���� ���� ����
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
                enemy?.TakeDamage(_damage);
                //Debug.Log($"{collision.name}가 {_damage}만큼 맞아서 {enemy.GetHealth()}");
                _lastHitTime[collision] = Time.time;

                Rigidbody2D rb = collision.attachedRigidbody;
                if (rb != null)
                {
                    Vector2 knockbackDir = transform.up; // źȯ�� ���ƿ� �ݴ� ����
                    rb.AddForce(knockbackDir * _statHandler.KnockbackForce, ForceMode2D.Impulse);
                }

                if (!CanPenetrate)
                    Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        foreach (var mod in _modules)
        {
            mod.OnUpdate(this);
        }

        // ȸ�� ������ forward ������ �������� �̵�
        transform.position += transform.up * Speed * Time.deltaTime;
    }

}
