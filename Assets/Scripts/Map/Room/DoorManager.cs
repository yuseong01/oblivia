using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : Singleton<DoorManager>
{
    public Vector2Int currentRoomPos;

    protected override void Awake()
    {
        base.Awake();
    }


}
