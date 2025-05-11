

// 퍼지는 탄환 모듈
using UnityEngine;

[CreateAssetMenu(menuName = "FireModules/Radial Shot")]
public class RadialShotData : FireModuleData
{
    public int ProjCount;

    public override IFireModule CreateInstance()
    {
        return new RadialShotModule(this);
    }
}