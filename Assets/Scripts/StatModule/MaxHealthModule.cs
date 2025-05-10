using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthModule : IStatModule
{

    private MaxHealthModuleData _data;

    public MaxHealthModule(MaxHealthModuleData data)
    {
        _data = data;
    }
    public void SetStat(PlayerStatHandler statHandler)
    {
        statHandler.MaxHealth = _data.MaxHealth;
    }
}
