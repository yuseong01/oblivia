using UnityEngine;

public abstract class StatModuleData : ScriptableObject
{
    public abstract IStatModule CreateInstance();
}

// ���� �ֱ� ����
[CreateAssetMenu(menuName = "StatModules/Attack Rate")]
public class AttackRateModuleData : StatModuleData
{
    public float Rate = 1f;

    public override IStatModule CreateInstance()
    {
        return new AttackRateModule(this);
    }
}