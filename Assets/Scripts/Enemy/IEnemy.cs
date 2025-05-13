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
        Elite,
        Teleport,
    }
    Transform GetPlayerPosition(); // �÷��̾� ��ġ
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
