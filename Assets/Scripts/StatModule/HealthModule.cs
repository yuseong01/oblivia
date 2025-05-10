using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModule :  IStatModule
{
    private HealthModuleData _data;

    public HealthModule(HealthModuleData data)
    {
        _data = data;
    }
    public void SetStat(PlayerStatHandler statHandler)
    {
        statHandler.Health = _data.Health;
    }
}
