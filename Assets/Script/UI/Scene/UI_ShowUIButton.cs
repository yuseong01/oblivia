using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UI_ShowUIButton : UI_Scene
{

    [SerializeField]
    private string uiName;
    private GameObject popupUI;
    [SerializeField]
    Vector3 pivotPos;
    enum Buttons
    {
        PointButton,

    }

    public override void Init()
    {
        base.Init();
        uiName = gameObject.name.Replace("Button", "UI");

        // 버튼과 바인드
        Bind<Button>(typeof(Buttons));

        Get<Button>((int)Buttons.PointButton).onClick.AddListener(OnPointButtonClicked);
    }

    protected void OnPointButtonClicked()
    {
        if(popupUI == null)
        {
            popupUI = UIManager.instance.ShowPopupUI(uiName);
        }
    }

}
