using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public Sprite Icon;

    public List<ProjectileModuleData> ProjectileModules;
    public List<FireModuleData> FireModules;
    
}
