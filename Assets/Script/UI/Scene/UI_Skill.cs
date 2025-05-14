using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill : UI_Scene
{
    public SkillController SkillController;
    private Skill _skill;
    private Image _skillIcon;
    private Image _skillBackIcon;
    enum Images
    {
        Skill,
        Back,
    }
 
    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        _skill = SkillController.GetSkill();

        _skillIcon = Get<Image>((int)Images.Skill);
        _skillIcon.sprite = _skill.Icon;

        _skillBackIcon = Get<Image>((int)Images.Back);
        _skillBackIcon.sprite = _skill.Icon;
    }

    public void ActiveSkillByButton()
    {
        SkillController.ActiveSkill();
    }

    private void Update()
    {
        if (_skill != null)
        {
            float cooldown = _skill.CooldownTime;
            float elapsed = Time.time - _skill.lastUsedTime;
            float ratio = Mathf.Clamp01(elapsed / cooldown); 
            _skillIcon.fillAmount = ratio;
        }
    }
}
