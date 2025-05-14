// ������ źȯ ���
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile/Homing")]
public class HomingData : ProjectileModuleData
{
    public override IProjectileModule CreateInstance()
    {
        return new HomingModule(this);
    }
}