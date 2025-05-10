using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialShotModule : MonoBehaviour, IFireModule
{
    private RadialShotData _radialShotData;

    public RadialShotModule(RadialShotData data)
    {
        _radialShotData = data;
    }

    public void OnFire(AttackController attack, Transform enemy = null) 
    {
        float angleStep = 360f / _radialShotData.ProjCount;
        Vector3 firePosition = attack.AttackPivot.transform.position;

        for (int i = 0; i < _radialShotData.ProjCount; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject go = GameObject.Instantiate(
                attack.ProjectilePrefab,
                firePosition,
                rotation
            );

            Vector2 direction = rotation * Vector2.up;
            var proj = go.GetComponent<Projectile>();
            proj.Init(attack.StatHandler, enemy, attack.ItemEffectManager.GetProjModules());
        }
    }
}
