using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationModule : IStatModule
{

    private DurationModuleData _data;

    public DurationModule(DurationModuleData data)
    {
        _data = data;
    }
    public void SetStat(PlayerStatHandler statHandler)
    {
        statHandler.AttackDuration = _data.Duration;
    }
}
