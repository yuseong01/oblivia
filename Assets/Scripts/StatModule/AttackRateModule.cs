using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRateModule : IStatModule
{    
    
    private AttackRateModuleData _attackRateData;

    public AttackRateModule(AttackRateModuleData data)
    {
        _attackRateData = data;
    }
    public void SetStat(PlayerStatHandler statHandler)
    {
        statHandler.AttackRate = _attackRateData.Rate;
    }
}
