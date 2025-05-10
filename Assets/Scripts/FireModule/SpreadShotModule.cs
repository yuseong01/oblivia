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
        Vector2 dir = (enemy.position - attack.AttackPivot.transform.position).normalized;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        for (int i = 0; i < _spreadShotData.ProjCount; i++)
        {
            float offset;

            if (_spreadShotData.ProjCount % 2 == 1)
            {
                // 홀수: 가운데 탄환은 정확히 타겟 향함
                int middle = _spreadShotData.ProjCount / 2;
                offset = (i - middle) * _spreadShotData.Angle;
            }
            else
            {
                // 짝수: 정확히 가운데는 피해서 발사 (빗겨감)
                offset = (i - (_spreadShotData.ProjCount - 1) / 2f) * _spreadShotData.Angle;

                // 가운데 각도(0도) 건너뛰기: 오프셋이 0이면 약간 밀어줌
                if (Mathf.Approximately(offset, 0f))
                    offset += _spreadShotData.Angle / 2f;
            }

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
