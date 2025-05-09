using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShotModule : MonoBehaviour, IFireModule
{
    private SpreadShotData _spreadShotData;

    public SpreadShotModule(SpreadShotData data)
    {
        _spreadShotData = data;
    }

    public void OnFire(AttackController attack, Transform enemy)
    {
        int middle = _spreadShotData.ProjCount / 2;

        // 중심 발사 방향 계산 (타겟을 향한 방향)
        Vector2 dir = (enemy.position - attack.AttackPivot.transform.position).normalized;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; // Z축 기준

        for (int i = 0; i < _spreadShotData.ProjCount; i++)
        {
            float offset = (i - middle) * _spreadShotData.Angle;
            Quaternion rotation = Quaternion.Euler(0, 0, baseAngle + offset);

            GameObject go = GameObject.Instantiate(
                attack.ProjectilePrefab,
                attack.AttackPivot.transform.position,
                rotation
            );

            var proj = go.GetComponent<Projectile>();
            proj.Init(attack.StatHandler, enemy, attack.ItemEffectManager.GetProjModules());
        }
    }

}
