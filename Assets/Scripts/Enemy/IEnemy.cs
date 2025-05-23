using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Enemy 데이터/행동
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
        Rush1,
        Rush2,
        Minion,
        Explode,
    }
    Transform GetPlayerPosition(); // 플레이어 위치
    SpriteRenderer GetSpriteRenderer(); // 플레이어 위치
    Transform GetEnemyPosition(); // Enemy 위치
    float GetPlayerHealth(); // 플레이어 체력
    bool CheckInPlayerInRanged(); // 플레이어와 가까운지 체크
    EnemyType GetEnemyType(); // Enemy type 체크
    Animator GetAnimator(); // Enemy 애니메이터 가져오기
    float GetHealth();
    float SetSpeed(float amount);
    float GetSpeed();
    void TakeDamage(float amount, Vector2 hitDirection);
    Room GetCurrentRoom();
    void SetCurrentRoom(Room room);
    float GetAttackPower();
    void SetStop(bool isStop);
}
