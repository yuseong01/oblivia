using UnityEngine;

public abstract class FireModuleData : ScriptableObject
{
    public abstract IFireModule CreateInstance();
}

// ������ źȯ ���
[CreateAssetMenu(menuName = "FireModules/Spread Shot")]
public class SpreadShotData : FireModuleData
{
    public float Angle = 15f;
    public int ProjCount;

    public override IFireModule CreateInstance()
    {
        return new SpreadShotModule(this);
    }
}

// ������ źȯ ���
[CreateAssetMenu(menuName = "FireModules/Radial Shot")]
public class RadialShotData : FireModuleData
{
    public int ProjCount;

    public override IFireModule CreateInstance()
    {
        return new RadialShotModule(this);
    }
}
