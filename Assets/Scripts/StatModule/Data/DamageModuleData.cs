// 탄환 데미지
using UnityEngine;

[CreateAssetMenu(menuName = "StatModules/Damage")]
public class DamageModuleData : StatModuleData
{
    public float Damage = 1;
    public override IStatModule CreateInstance()
    {
        return new DamageModule(this);
    }
}
