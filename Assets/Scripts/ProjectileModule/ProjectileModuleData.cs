using UnityEngine;

public abstract class ProjectileModuleData : ScriptableObject
{
    public abstract IProjectileModule CreateInstance();
}
