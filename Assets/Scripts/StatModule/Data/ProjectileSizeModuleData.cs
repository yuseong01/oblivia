// ÅºÈ¯ Å©±â
using UnityEngine;

[CreateAssetMenu(menuName = "StatModules/Projectile Size")]
public class ProjectileSizeModuleData : StatModuleData
{
    public float Size = 1;
    public override IStatModule CreateInstance()
    {
        return new PojectileSizeModule(this);
    }
}