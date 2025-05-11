using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PojectileSizeModule : MonoBehaviour, IStatModule
{
    private ProjectileSizeModuleData _data;

    public PojectileSizeModule(ProjectileSizeModuleData data)
    {
        _data = data;
    }
    public void SetStat(PlayerStatHandler statHandler)
    {
        statHandler.ProjectileSize = _data.Size;
    }
}
