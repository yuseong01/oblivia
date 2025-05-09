using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrateModule : MonoBehaviour, IProjectileModule
{
    private PenetrateData _penetrateData;

    public PenetrateModule(PenetrateData data)
    {
        _penetrateData = data;
    }
    public void OnFire(Projectile projectile)
    {
        projectile.CanPenetrate = true;
    }

    public void OnUpdate(Projectile projectile)
    {

    }
}
