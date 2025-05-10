using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCountModule : IStatModule
{
    private AttackCountModuleData _attackCountData;

    public AttackCountModule(AttackCountModuleData data)
    {
        _attackCountData = data;
    }
    public void SetStat(PlayerStatHandler statHandler)
    {
        statHandler.AttackCount = _attackCountData.Count;
    }
}

