using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemDescription : UI_Popup
{
    public ItemData Itemdata;

    enum Imanges
    {
        Icon
    }
    enum TMPs
    {
        Name,
        Description
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Imanges));
        Bind<TextMeshProUGUI>(typeof(TMPs));

        Get<Image>((int)Imanges.Icon).sprite = Itemdata.Icon;

        Get<TextMeshProUGUI>((int)TMPs.Name).text = Itemdata.ItemName;
        Get<TextMeshProUGUI>((int)TMPs.Description).text = Itemdata.Description;

        StartCoroutine(EndPopUp());
    }

    IEnumerator EndPopUp()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        
    }

}
