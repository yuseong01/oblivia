using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UI_Scene
{
    public PlayerStatHandler Stathandler;

    enum Images
    {
        EmptyBar,
        HpBar,
    }
 
    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));

        UpdateHealthBar(Stathandler.Health);
        UpdateMaxHealthBar(Stathandler.MaxHealth);
    }

    void OnEnable()
    {
        Stathandler.OnHealthChanged += UpdateHealthBar;
        Stathandler.OnMaxHealthChanged += UpdateMaxHealthBar;
    }

    void OnDisable()
    {
        Stathandler.OnHealthChanged -= UpdateHealthBar;
        Stathandler.OnMaxHealthChanged -= UpdateMaxHealthBar;
    }

    void UpdateHealthBar(float current)
    {
        var image = Get<Image>((int)Images.HpBar);
        if (image != null)
            image.fillAmount = (float)current / Stathandler.limitHealth;
    }
    void UpdateMaxHealthBar(float current)
    {
        var image = Get<Image>((int)Images.EmptyBar);
        if (image != null)
            image.fillAmount = (float)current / Stathandler.limitHealth;
    }
}
