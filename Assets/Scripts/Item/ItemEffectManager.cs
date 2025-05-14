using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEffectManager : MonoBehaviour
{
    private PlayerStatHandler _statHandler;
    private List<IProjectileModule> activeModules = new List<IProjectileModule>();
    private List<IFireModule> fireModules = new List<IFireModule>();
    [SerializeField] private GameObject _UIPrefab;
    [SerializeField] private GameObject _ItemUIParent;
    [SerializeField] private GameObject _ItemUIPrefab;


    private void Awake()
    {
        _statHandler = GetComponent<PlayerStatHandler>();
    }

    public void AddItem(ItemData item)
    {
        var go =  Instantiate(_UIPrefab);
        var go2 = Instantiate(_ItemUIPrefab);

        var image = go2.GetComponent<Image>();
        image.sprite = item.Icon;

        go2.transform.SetParent(_ItemUIParent.transform);
        go2.transform.localScale = Vector3.one;
        UI_ItemDescription descriptionUI = go.GetComponent<UI_ItemDescription>();
        if (descriptionUI != null)
        {
            descriptionUI.Itemdata = item;
        }
        descriptionUI.gameObject.SetActive(true);


        foreach (var projModuleData in item.ProjectileModules)
        {
            activeModules.Add(projModuleData.CreateInstance());
        }
        foreach(var fireData in item.FireModules)
        {
            fireModules.Add(fireData.CreateInstance());
        }
        foreach(var statData in item.StatModules)
        {
          statData.CreateInstance().SetStat(_statHandler);
        }
    }

    public List<IProjectileModule> GetProjModules()
    {
        var clones = new List<IProjectileModule>();
        foreach (var module in activeModules)
            clones.Add(module.Clone());
        return clones;
    }
    public List<IFireModule> GetFireModules() => fireModules;
}
