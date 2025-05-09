using UnityEngine;

public abstract class FireModuleData : ScriptableObject
{
    public abstract IFireModule CreateInstance();
}

// 퍼지는 탄환 모듈
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

// 퍼지는 탄환 모듈
[CreateAssetMenu(menuName = "FireModules/Radial Shot")]
public class RadialShotData : FireModuleData
{
    public int ProjCount;

    public override IFireModule CreateInstance()
    {
        return new RadialShotModule(this);
    }
}
