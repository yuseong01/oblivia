using UnityEngine;

public abstract class StatModuleData : ScriptableObject
{
    public abstract IStatModule CreateInstance();
}

// źȯ ������
[CreateAssetMenu(menuName = "StatModules/Damage")]
public class DamageModuleData : StatModuleData
{
    public float Damage = 1;
    public override IStatModule CreateInstance()
    {
        return new DamageModule(this);
    }
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

// źȯ Ƚ��
[CreateAssetMenu(menuName = "StatModules/Attack Count")]
public class AttackCountModuleData : StatModuleData
{
    public int Count = 1;
    public override IStatModule CreateInstance()
    {
        return new AttackCountModule(this);
    }
}

// źȯ ũ��
[CreateAssetMenu(menuName = "StatModules/Projectile Size")]
public class ProjectileSizeModuleData : StatModuleData
{
    public float Size = 1;
    public override IStatModule CreateInstance()
    {
        return new PojectileSizeModule(this);
    }
}