// ���� źȯ ���
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile/boomerangModule")]
public class BoomerangData : ProjectileModuleData
{
    public override IProjectileModule CreateInstance()
    {
        return new BoomerangModule(this);
    }
}