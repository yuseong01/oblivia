using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill : UI_Scene
{
    public SkillController SkillController;
    private Skill _skill;
    private Image _skillIcon;
    enum Images
    {
        Skill,
    }
 
    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        _skill = SkillController.GetSkill();

        _skillIcon = Get<Image>((int)Images.Skill);
        _skillIcon.sprite = _skill.Icon;
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
