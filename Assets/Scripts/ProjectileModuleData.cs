using UnityEngine;

[CreateAssetMenu(menuName ="Projectile/Module data")]
public class ProjectileModuleData : ScriptableObject
{
    public string ModuleName;
    public GameObject ModulePrefab;// 반드시 IProjectileModule이 붙어야함
}
