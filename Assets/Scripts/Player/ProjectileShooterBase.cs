using UnityEngine;

public abstract class ProjectileShooterBase : MonoBehaviour
{
    public abstract GameObject ProjectilePrefab { get; set; }
    public abstract GameObject AttackPivot { get; set; }
    public abstract PlayerStatHandler StatHandler { get; set; }
    public abstract ItemEffectManager ItemEffectManager { get; set; }

    public void OnFire(Transform enemy, GameObject projectilePrefab = null)
    {
        var obj = ProjectilePrefab;
        if (projectilePrefab != null)
            obj = projectilePrefab;

        Vector2 dir = (enemy.position - AttackPivot.transform.position).normalized;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        for (int i = 0; i < StatHandler.AttackCount; i++)
        {
            float offset;

            if (StatHandler.AttackCount % 2 == 1)
            {
                int middle = StatHandler.AttackCount / 2;
                offset = (i - middle) * StatHandler.AttackAngle;
            }
            else
            {
                offset = (i - (StatHandler.AttackCount - 1) / 2f) * StatHandler.AttackAngle;
                if (Mathf.Approximately(offset, 0f))
                    offset += StatHandler.AttackAngle / 2f;
            }

            Quaternion rotation = Quaternion.Euler(0, 0, baseAngle + offset);

            GameObject go = GameObject.Instantiate(
                obj,
                AttackPivot.transform.position,
                rotation
            );

            var proj = go.GetComponent<Projectile>();
            proj.Init(StatHandler, enemy, ItemEffectManager.GetProjModules());
        }
    }
}
