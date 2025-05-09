using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectManager : MonoBehaviour
{
    private List<IProjectileModule> activeModules = new List<IProjectileModule>();
    private List<IFireModule> fireModules = new List<IFireModule>();

    public void AddItem(ItemData item)
    {
        foreach(var projModuleData in item.ProjectileModules)
        {
            GameObject go = Instantiate(projModuleData.ModulePrefab);
            IProjectileModule module = go.GetComponent<IProjectileModule>();
            activeModules.Add(module);
        }
        foreach(var fireData in item.FireModules)
        {
            fireModules.Add(fireData.CreateInstance());
        }
    }

    public List<IProjectileModule> GetProjModules()
    {
        return new List<IProjectileModule>(activeModules);
    }

    public List<IFireModule> GetFireModules() => fireModules;
}
