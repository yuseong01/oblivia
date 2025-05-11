// 공격 주기 관련
using UnityEngine;

[CreateAssetMenu(menuName = "StatModules/Attack Rate")]
public class AttackRateModuleData : StatModuleData
{
    public float Rate = 1f;
    public override IStatModule CreateInstance()
    {
        return new AttackRateModule(this);
    }
}