using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HomingModule : MonoBehaviour, IProjectileModule
{
    private Transform _target;
    private HomingData _homingData;
    public IProjectileModule Clone() => new HomingModule(_homingData);
    public HomingModule(HomingData data)
    {
        _homingData = data;
    }

    public void OnFire(Projectile projectile)
    {
        _target = projectile.Target;
    }

    public void OnUpdate(Projectile projectile)
    {
        if (_target == null) return;

        Vector3 dir = (_target.position - projectile.transform.position).normalized;
        Quaternion rotate = Quaternion.LookRotation(Vector3.forward, dir);
        projectile.transform.rotation = Quaternion.Lerp(
            projectile.transform.rotation, rotate, Time.deltaTime * projectile.Speed);
    }

}
