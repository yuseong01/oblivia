using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrateModule : MonoBehaviour, IProjectileModule
{

    public void OnFire(Projectile projectile)
    {
        projectile.CanPenetrate = true;
    }

    public void OnUpdate(Projectile projectile)
    {

    }
}
