using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public Sprite Icon;
    public float CooldownTime = 1f;

    [System.NonSerialized] public float lastUsedTime = -999;

    public bool IsReady => Time.time >= lastUsedTime + CooldownTime;

    public  bool TryActivateSkill(AttackController caster)
    {
        if (IsReady)
        {
            ActivateSkill(caster);
            lastUsedTime = Time.time;
            return true;
        }
        return false;
    }

    protected abstract void ActivateSkill(AttackController caster);
}
