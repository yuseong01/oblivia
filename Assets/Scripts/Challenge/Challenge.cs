using System;

[Serializable]
public enum ChallengeType
{
    CountBased, // 카운트
    ConditionBased // 조건
}

[Serializable]
public class Challenge
{
    public string id;               // 도전과제 ID
    public string description;      // 설명
    public int goal;                // 목표
    public int currentCount;        // 현재 진행도
    public bool isCompleted;        // 완료 여부
    public ChallengeType type;      // 도전과제 타입

    public string rewardCharacterId; // 이 도전과제 완료 시 해금될 캐릭터 ID
}
