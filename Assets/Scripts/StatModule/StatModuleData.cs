using UnityEngine;

public abstract class StatModuleData : ScriptableObject
{
    public abstract IStatModule CreateInstance();
}

// ÅºÈ¯ µ¥¹ÌÁö
[CreateAssetMenu(menuName = "StatModules/Damage")]
public class DamageModuleData : StatModuleData
{
    public float Damage = 1;
    public override IStatModule CreateInstance()
    {
        return new DamageModule(this);
    }
}

// °ø°Ý ÁÖ±â °ü·Ã
[CreateAssetMenu(menuName = "StatModules/Attack Rate")]
public class AttackRateModuleData : StatModuleData
{
    public float Rate = 1f;
    public override IStatModule CreateInstance()
    {
        return new AttackRateModule(this);
    }
}

// ÅºÈ¯ È½¼ö
[CreateAssetMenu(menuName = "StatModules/Attack Count")]
public class AttackCountModuleData : StatModuleData
{
    public int Count = 1;
    public override IStatModule CreateInstance()
    {
        return new AttackCountModule(this);
    }
}

// ÅºÈ¯ Å©±â
[CreateAssetMenu(menuName = "StatModules/Projectile Size")]
public class ProjectileSizeModuleData : StatModuleData
{
    public float Size = 1;
    public override IStatModule CreateInstance()
    {
        return new PojectileSizeModule(this);
    }
}