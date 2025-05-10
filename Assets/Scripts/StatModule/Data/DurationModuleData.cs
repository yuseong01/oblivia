// 공격 유지시간
using UnityEngine;

[CreateAssetMenu(menuName = "StatModules/Duration")]
public class DurationModuleData : StatModuleData
{
    public float Duration = 1;
    public override IStatModule CreateInstance()
    {
        return new DurationModule(this);
    }
}