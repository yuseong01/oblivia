// ���� źȯ ���
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile/Penetrate")]
public class PenetrateData : ProjectileModuleData
{
    public override IProjectileModule CreateInstance()
    {
        return new PenetrateModule(this);
    }
}