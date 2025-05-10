using UnityEngine;

public abstract class StatModuleData : ScriptableObject
{
    public abstract IStatModule CreateInstance();
}

// 탄환 데미지
[CreateAssetMenu(menuName = "StatModules/Damage")]
public class DamageModuleData : StatModuleData
{
    public float Damage = 1;
    public override IStatModule CreateInstance()
    {
        return new DamageModule(this);
    }
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

// 탄환 횟수
[CreateAssetMenu(menuName = "StatModules/Attack Count")]
public class AttackCountModuleData : StatModuleData
{
    public int Count = 1;
    public override IStatModule CreateInstance()
    {
        return new AttackCountModule(this);
    }
}

// 탄환 크기
[CreateAssetMenu(menuName = "StatModules/Projectile Size")]
public class ProjectileSizeModuleData : StatModuleData
{
    public float Size = 1;
    public override IStatModule CreateInstance()
    {
        return new PojectileSizeModule(this);
    }
}

// 공격 유지시간
[CreateAssetMenu(menuName = "StatModules/Duration")]
public class DurationModuleData : StatModuleData
{
    public float Duration = 1;
    public override IStatModule CreateInstance()
    {
        return new DurationModule(this);
    }
}

// 공격 사거리(인식 범위)
[CreateAssetMenu(menuName = "StatModules/Attack Range")]
public class AttackRangeModuleData : StatModuleData
{
    public float Range = 1;
    public override IStatModule CreateInstance()
    {
        return new AttackRangeModule(this);
    }
}

// 현재 체력 관련
[CreateAssetMenu(menuName = "StatModules/Health")]
public class HealthModuleData : StatModuleData
{
    public float Health = 1;
    public override IStatModule CreateInstance()
    {
        return new HealthModule(this);
    }
}

// 최대 체력 관련
[CreateAssetMenu(menuName = "StatModules/MaxHealth")]
public class MaxHealthModuleData : StatModuleData
{
    public float MaxHealth = 1;
    public override IStatModule CreateInstance()
    {
        return new MaxHealthModule(this);
    }
}

// 이동 속도 관련
[CreateAssetMenu(menuName = "StatModules/Move Speed")]
public class MoveSpeedData : StatModuleData
{
    public float MoveSpeed = 1;
    public override IStatModule CreateInstance()
    {
        return new MoveSpeedModule(this);
    }
}