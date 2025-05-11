using UnityEngine;

public abstract class StatModuleData : ScriptableObject
{
    public abstract IStatModule CreateInstance();
}