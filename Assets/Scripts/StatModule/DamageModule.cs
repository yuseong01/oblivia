using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageModule : MonoBehaviour, IStatModule
{
    private DamageModuleData _data;

    public DamageModule(DamageModuleData data)
    {
        _data = data;
    }
    public void SetStat(PlayerStatHandler statHandler)
    {
        statHandler.Damage = _data.Damage;
    }
}
