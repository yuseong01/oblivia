using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCountModule : IStatModule
{
    private OrbitCountModuleData _data;

    public OrbitCountModule(OrbitCountModuleData data)
    {
        _data = data;
    }
    public void SetStat(PlayerStatHandler statHandler)
    {
        statHandler.OrbitCount = _data.OrbitCount;
    }
}
