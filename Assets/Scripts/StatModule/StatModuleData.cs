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

// ���� �����ð�
[CreateAssetMenu(menuName = "StatModules/Duration")]
public class DurationModuleData : StatModuleData
{
    public float Duration = 1;
    public override IStatModule CreateInstance()
    {
        return new DurationModule(this);
    }
}

// ���� ��Ÿ�(�ν� ����)
[CreateAssetMenu(menuName = "StatModules/Attack Range")]
public class AttackRangeModuleData : StatModuleData
{
    public float Range = 1;
    public override IStatModule CreateInstance()
    {
        return new AttackRangeModule(this);
    }
}

// ���� ü�� ����
[CreateAssetMenu(menuName = "StatModules/Health")]
public class HealthModuleData : StatModuleData
{
    public float Health = 1;
    public override IStatModule CreateInstance()
    {
        return new HealthModule(this);
    }
}

// �ִ� ü�� ����
[CreateAssetMenu(menuName = "StatModules/MaxHealth")]
public class MaxHealthModuleData : StatModuleData
{
    public float MaxHealth = 1;
    public override IStatModule CreateInstance()
    {
        return new MaxHealthModule(this);
    }
}

// �̵� �ӵ� ����
[CreateAssetMenu(menuName = "StatModules/Move Speed")]
public class MoveSpeedData : StatModuleData
{
    public float MoveSpeed = 1;
    public override IStatModule CreateInstance()
    {
        return new MoveSpeedModule(this);
    }
}