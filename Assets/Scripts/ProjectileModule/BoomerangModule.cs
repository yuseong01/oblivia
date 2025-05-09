using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangModule : MonoBehaviour, IProjectileModule
{
    private Vector3 _origin;
    private bool _returning = false;
    private float _flightTime = 0f;
    private BoomerangData _data;

    public BoomerangModule(BoomerangData data)
    {
        _data = data;
    }

    public IProjectileModule Clone() => new BoomerangModule(_data);
    public void OnFire(Projectile projectile)
    {
        _origin = projectile.transform.position;
        _flightTime = 0f;
        _returning = false;
    }

    public void OnUpdate(Projectile projectile)
    {
        _flightTime += Time.deltaTime;


        if (!_returning && _flightTime >= projectile.AttackDuration / 2)
        {
            _returning = true;
        }

        // 이동 방향 설정
        Vector3 targetPos = _returning ? _origin : projectile.transform.position + projectile.transform.up * projectile.Speed * Time.deltaTime;
        Vector3 dir = (targetPos - projectile.transform.position).normalized;

        // 회전 처리
        Quaternion rotate = Quaternion.LookRotation(Vector3.forward, dir);
        projectile.transform.rotation = Quaternion.Lerp(projectile.transform.rotation, rotate, Time.deltaTime * projectile.Speed*2);

    }
}
