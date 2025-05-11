using UnityEngine;

public abstract class FireModuleData : ScriptableObject
{
    public abstract IFireModule CreateInstance();
}
