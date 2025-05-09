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

    public void Init(float damage, float speed, Transform enemyTransform, List<IProjectileModule> modules)
    {
        _damage = damage;
        Speed = speed;
        _modules = modules;
        Target = enemyTransform;

        foreach (var mod in modules)
        {
            mod.OnFire(this);
        }

        //테스트 삭제 추후 제거
        Destroy(gameObject, 3f);
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
