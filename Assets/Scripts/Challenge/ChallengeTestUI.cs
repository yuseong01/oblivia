using UnityEngine;

public class ChallengeTestUI : MonoBehaviour
{
    public ChallengeManager challengeManager;

    public void OnKillMonsterButtonClicked()
    {
        challengeManager.IncreaseProgress("kill_monster", 1);
    }
}
