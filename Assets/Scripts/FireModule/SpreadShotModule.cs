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
                // Ȧ��: ��� źȯ�� ��Ȯ�� Ÿ�� ����
                int middle = _spreadShotData.ProjCount / 2;
                offset = (i - middle) * _spreadShotData.Angle;
            }
            else
            {
                // ¦��: ��Ȯ�� ����� ���ؼ� �߻� (���ܰ�)
                offset = (i - (_spreadShotData.ProjCount - 1) / 2f) * _spreadShotData.Angle;

                // ��� ����(0��) �ǳʶٱ�: �������� 0�̸� �ణ �о���
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
