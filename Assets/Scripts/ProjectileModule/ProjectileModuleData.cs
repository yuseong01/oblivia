using UnityEngine;

public abstract class ProjectileModuleData : ScriptableObject
{
    public abstract IProjectileModule CreateInstance();
}


// 퍼지는 탄환 모듈
[CreateAssetMenu(menuName = "Projectile/Homing")]
public class HomingData : ProjectileModuleData
{
    public override IProjectileModule CreateInstance()
    {
        return new HomingModule(this);
    }
}

// 관통 탄환 모듈
[CreateAssetMenu(menuName = "Projectile/Penetrate")]
public class PenetrateData : ProjectileModuleData
{
    public override IProjectileModule CreateInstance()
    {
        return new PenetrateModule(this);
    }
}