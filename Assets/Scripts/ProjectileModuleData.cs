using UnityEngine;

[CreateAssetMenu(menuName ="Projectile/Module data")]
public class ProjectileModuleData : ScriptableObject
{
    public string ModuleName;
    public GameObject ModulePrefab;// �ݵ�� IProjectileModule�� �پ����
}
