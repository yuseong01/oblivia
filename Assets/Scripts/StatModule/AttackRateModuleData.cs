using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatModules/Attack Rate")]
public class AttackRateModuleData : StatModuleData
{
    public float Rate = 1f;

    public override IStatModule CreateInstance()
    {
        return new AttackRateModule(this);
    }
}
