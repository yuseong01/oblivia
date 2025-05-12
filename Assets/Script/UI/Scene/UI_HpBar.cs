using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UI_Scene
{
    public PlayerStatHandler Stathandler;
    private Image _healthSlider;

    enum Images
    {
        EmptyBar,
        HpBar,
    }
 
    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));

        // Get<Button>((int)Buttons.NomalModeButton).onClick.AddListener(OnNomalButtonClick);
        //  Get<Button>((int)Buttons.HardModeButton).onClick.AddListener(OnHardButtonClick);
        //  Get<Button>((int)Buttons.QuitButton).onClick.AddListener(OnQuitButtonClick);
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
