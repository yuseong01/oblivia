using UnityEngine;

public class ChallengeTestUI : MonoBehaviour
{
    public ChallengeManager challengeManager;

    void Awake()
    {
        if (challengeManager == null)
        {
            challengeManager = ChallengeManager.Instance;
        }
    }
    public void OnKillMonsterButtonClicked()
    {
        challengeManager.IncreaseProgress("kill_monster", 1);
    }
}
