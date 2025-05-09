using UnityEngine;

public abstract class StatModuleData : ScriptableObject
{
    public abstract IStatModule CreateInstance();
}

// 공격 주기 관련
[CreateAssetMenu(menuName = "StatModules/Attack Rate")]
public class AttackRateModuleData : StatModuleData
{
    public float Rate = 1f;
    public override IStatModule CreateInstance()
    {
        return new AttackRateModule(this);
    }
}

[CreateAssetMenu(menuName = "StatModules/Attack Count")]
public class AttackCountModuleData : StatModuleData
{
    public int Count = 1;
    public override IStatModule CreateInstance()
    {
        return new AttackCountModule(this);
    }
}