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
        CloneBoss,
        Ranged,
        Elite,
    }
    Transform GetPlayerPosition(); // �÷��̾� ��ġ
    Transform GetEnemyPosition(); // Enemy ��ġ
    float GetPlayerHealth(); // �÷��̾� ü��
    bool CheckInPlayerInRanged(); // �÷��̾�� ������� üũ
    EnemyType GetEnemyType(); // Enemy type üũ
    Animator GetAnimator(); // Enemy �ִϸ����� ��������
    float GetHealth();
    void TakeDamage(int amount);
}
