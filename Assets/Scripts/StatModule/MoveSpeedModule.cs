using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedModule : IStatModule
{
    private MoveSpeedData _data;

    public MoveSpeedModule(MoveSpeedData data)
    {
        _data = data;
    }
    public void SetStat(PlayerStatHandler statHandler)
    {
        statHandler.MoveSpeed = _data.MoveSpeed;
    }
}
