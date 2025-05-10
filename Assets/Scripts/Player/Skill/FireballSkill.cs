using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "Skill/Fireball")]
public class FireballSkill : Skill
{
    public GameObject FireballPrefab;
    public float Speed = 5f;
    public float ExtraDamage = 10f; // �߰��� ������ ����
    public float ExtraSize = 0.2f; // �߰��� ������ ����

    protected override void ActivateSkill(AttackController controller)
    {
        Collider2D[] cor = controller.CheckEnemy();
        Vector2 dir = (cor[0].transform.position - controller.AttackPivot.transform.position).normalized;

        GameObject fireball = Instantiate(FireballPrefab, controller.transform.position, Quaternion.identity);
        fireball.transform.up = dir;

        // ���� ������ Ŭ�������� ���纻 ����
        PlayerStatHandler clonedStats = controller.GetComponent<PlayerStatHandler>().Clone();
        clonedStats.Damage = ExtraDamage; // �߰� ������ ����
        clonedStats.ProjectileSize = ExtraSize; // �߰� ũ�� ����
        Projectile projectile = fireball.GetComponent<Projectile>();
        if (projectile != null)
        {
            List<IProjectileModule> modules = new List<IProjectileModule>(); // ����� �ִٸ� �߰�
            projectile.Init(clonedStats, cor[0].transform, modules); // PlayerStats ���
        }
    }

}
