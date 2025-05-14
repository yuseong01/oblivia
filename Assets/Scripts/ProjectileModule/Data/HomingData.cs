// 퍼지는 탄환 모듈
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile/Homing")]
public class HomingData : ProjectileModuleData
{
    public override IProjectileModule CreateInstance()
    {
        return new HomingModule(this);
    }
}