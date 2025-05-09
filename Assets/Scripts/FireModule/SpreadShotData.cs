using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
