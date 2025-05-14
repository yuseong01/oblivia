// 최대 체력 관련
using UnityEngine;

[CreateAssetMenu(menuName = "StatModules/MaxHealth")]
public class MaxHealthModuleData : StatModuleData
{
    public float MaxHealth = 1;
    public override IStatModule CreateInstance()
    {
        return new MaxHealthModule(this);
    }
}