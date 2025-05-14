using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField] private Skill _skill;
    [SerializeField] private KeyCode _key = KeyCode.Space;
    private AttackController _attackController;

    private void Awake()
    {
        _attackController = GetComponent<AttackController>();
    }

    void Update()
    {
        if (_skill != null && Input.GetKeyDown(_key))
        {
            Collider2D[] cor = _attackController.CheckEnemy();
            if(cor.Length > 0)
            {
                _skill.TryActivateSkill(_attackController);
            }
        }
    }

    // 스킬 교체
    public void SetSkill(Skill newSkill)
    {
        _skill = newSkill;
    }

    public Skill GetSkill()
    {
        return _skill;
    }
}
