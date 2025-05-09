using UnityEngine;

public abstract class ProjectileModuleData : ScriptableObject
{
    public abstract IProjectileModule CreateInstance();
}


// ������ źȯ ���
[CreateAssetMenu(menuName = "Projectile/Homing")]
public class HomingData : ProjectileModuleData
{
    public override IProjectileModule CreateInstance()
    {
        return new HomingModule(this);
    }
}

// ���� źȯ ���
[CreateAssetMenu(menuName = "Projectile/Penetrate")]
public class PenetrateData : ProjectileModuleData
{
    public override IProjectileModule CreateInstance()
    {
        return new PenetrateModule(this);
    }
}