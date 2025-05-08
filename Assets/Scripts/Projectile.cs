using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _speed;
    private float _damage;
    private List<IProjectileModule> _modules;
    private Vector2 _direction;

    public void Init(float damage, float speed, Vector2 direction, List<IProjectileModule> modules)
    {
        this._damage = damage;
        this._speed = speed;
        this._modules = modules;

        transform.up = direction;

        foreach (var mod in modules)
        {
            //mod.OnFire(this);
        }
    }

    void Update()
    {
        transform.position += transform.up * _speed * Time.deltaTime;

        foreach (var mod in _modules)
        {
          //  mod.OnUpdate(this);
        }
    }
}
