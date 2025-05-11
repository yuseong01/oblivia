using System;

[Serializable]
public enum ChallengeType
{
    CountBased, // ī��Ʈ
    ConditionBased // ����
}

[Serializable]
public class Challenge
{
    public string id;               // �������� ID
    public string description;      // ����
    public int goal;                // ��ǥ
    public int currentCount;        // ���� ���൵
    public bool isCompleted;        // �Ϸ� ����
    public ChallengeType type;      // �������� Ÿ��

    public string rewardCharacterId; // �� �������� �Ϸ� �� �رݵ� ĳ���� ID
}
