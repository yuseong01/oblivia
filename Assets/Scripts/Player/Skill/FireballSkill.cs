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
    public float ExtraDamage = 10f; // 추가된 데미지 변수
    public float ExtraSize = 0.2f; // 추가된 데미지 변수

    protected override void ActivateSkill(AttackController controller)
    {
        Collider2D[] cor = controller.CheckEnemy();
        Vector2 dir = (cor[0].transform.position - controller.AttackPivot.transform.position).normalized;

        GameObject fireball = Instantiate(FireballPrefab, controller.transform.position, Quaternion.identity);
        fireball.transform.up = dir;

        // 순수 데이터 클래스에서 복사본 생성
        PlayerStatHandler clonedStats = controller.GetComponent<PlayerStatHandler>().Clone();
        clonedStats.Damage = ExtraDamage; // 추가 데미지 적용
        clonedStats.ProjectileSize = ExtraSize; // 추가 크기 적용
        Projectile projectile = fireball.GetComponent<Projectile>();
        if (projectile != null)
        {
            List<IProjectileModule> modules = new List<IProjectileModule>(); // 모듈이 있다면 추가
            projectile.Init(clonedStats, cor[0].transform, modules); // PlayerStats 사용
        }
    }

}
