using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IEnemy;
using static PatternUtil;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;

public static class AttackDescison<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    public static bool TryDecideAttack(T enemy, out IAttackStrategy behavior)
    {
        behavior = null;
        float rand = Random.value;
        bool isRush = true;
        switch (enemy.GetEnemyType())
        {
            case EnemyType.Boss:
                if (enemy.GetHealth() <= 50f)
                {
                    if (rand < 0.6f) { behavior = new RadialAttack(); return true; }
                    if (rand < 0.85f) { behavior = new RushAttack(); return true; }
                    behavior = new LaserAttack(LaserPatternType.Radial); return true;
                }
                else
                {
                    if (rand < 0.3f) { behavior = new RangedAttack(); return true; }
                    if (rand < 0.7f) { behavior = new LaserAttack(LaserPatternType.Cross); return true; }
                    behavior = new RushAttack(); return true;
                }
            case EnemyType.Elite1: //
                if (rand < 0.4f) { behavior = new LaserAttack(LaserPatternType.Single); return true; }
                if (rand < 0.8f) { behavior = new LaserAttack(LaserPatternType.Diagonal); return true; }
                behavior = new RushAttack(); return true;
            case EnemyType.Elite2:
                if (enemy.GetHealth() > 50f)
                {
                    if (rand < 0.3f) { behavior = new RangedAttack(); return true; }
                    if (rand < 0.6f) { behavior = new LaserAttack(LaserPatternType.Diagonal); return true; }
                    behavior = new RushAttack(); return true;
                }
                else // 체력 50% 이하
                {
                    if (rand < 0.4f) { behavior = new RadialAttack(); return true; }
                    if (rand < 0.8f) { behavior = new LaserAttack(LaserPatternType.Cross); return true; }
                    behavior = new RushAttack(); return true;
                }
            case EnemyType.Rush1:
            case EnemyType.Rush2:
                if (isRush)
                {
                    behavior = new RushAttack();
                    isRush = false;
                    return true;
                }
                else
                {
                    isRush = true;
                    return false;
                }
                

        }

        return false;
    }

}
