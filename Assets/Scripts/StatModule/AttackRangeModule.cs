using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeModule : IStatModule
{

    private AttackRangeModuleData _data;

    public AttackRangeModule(AttackRangeModuleData data)
    {
        _data = data;
    }
    public void SetStat(PlayerStatHandler statHandler)
    {
        statHandler.AttackRange = _data.Range;
    }

}
