using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Enemy ������/�ൿ
public interface IEnemy 
{
    public enum EnemyType
    {
        Normal,
        Flee,
        Boss,
        Clone,
        Ranged,
        Elite1,
        Elite2,
        Teleport,
        Rush,
        Minion,
        Explode,
    }
    Transform GetPlayerPosition(); // �÷��̾� ��ġ
    SpriteRenderer GetSpriteRenderer(); // �÷��̾� ��ġ
    Transform GetEnemyPosition(); // Enemy ��ġ
    float GetPlayerHealth(); // �÷��̾� ü��
    bool CheckInPlayerInRanged(); // �÷��̾�� ������� üũ
    EnemyType GetEnemyType(); // Enemy type üũ
    Animator GetAnimator(); // Enemy �ִϸ����� ��������
    float GetHealth();
    float SetSpeed(float amount);
    float GetSpeed();
    void TakeDamage(float amount);
    Room GetCurrentRoom();
    void SetCurrentRoom(Room room);
    float GetAttackPower();
}
