using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager Instance { get; private set; }

    public ChallengeDatabase database;
    public GameObject rewardPanel;
    public TextMeshProUGUI rewardText;

    private List<Challenge> challenges = new List<Challenge>();
    private string savePath;

    void Awake()
    {
        #region 싱글톤
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        savePath = Application.persistentDataPath + "/challenges.json";
        LoadChallenges();
    }

    public void IncreaseProgress(string id, int amount)
    {
        foreach (Challenge challenge in challenges)
        {
            if (challenge.id == id && !challenge.isCompleted && challenge.type == ChallengeType.CountBased)
            {
                challenge.currentCount += amount;

                if (challenge.currentCount >= challenge.goal)
                {
                    challenge.isCompleted = true;
                    ShowReward(challenge);
                }

                break;
            }
        }

        SaveChallenges();
    }
    // 사용 예시 : ChallengeManager.Instance.IncreaseProgress("kill_monsters", 1);

    public void CompleteConditionChallenge(string id)
    {
        foreach (Challenge challenge in challenges)
        {
            if (challenge.id == id && !challenge.isCompleted && challenge.type == ChallengeType.ConditionBased)
            {
                challenge.isCompleted = true;
                ShowReward(challenge);

                // 도전과제 완료 시 캐릭터 해금
                if (!string.IsNullOrEmpty(challenge.rewardCharacterId))
                {
                    // 캐릭터 해금 요청
                    CharacterManager.Instance.UnlockCharacter(challenge.rewardCharacterId);
                }

                SaveChallenges();
                break;
            }
        }
    }

    // 사용 예시:
    /*
        ChallengeManager.Instance.CompleteConditionChallenge("boss_nodamage");
    */

    void ShowReward(Challenge challenge)
    {
        rewardText.text = $"Mission Complete!\n{challenge.description}";
        rewardPanel.SetActive(true);

        // 2초 후에 판넬과 텍스트를 비활성화
        StartCoroutine(HideRewardAfterDelay(2f));
    }

    IEnumerator HideRewardAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 딜레이 기다리기
        rewardPanel.SetActive(false);  // 판넬 숨기기
        rewardText.text = "";          // 텍스트 비우기
    }

    void SaveChallenges()
    {
        string json = JsonUtility.ToJson(new Wrapper { challenges = this.challenges }, true);
        File.WriteAllText(savePath, json);
    }

    void LoadChallenges()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            challenges = JsonUtility.FromJson<Wrapper>(json).challenges;
        }
        else
        {
            foreach (var c in database.challenges)
            {
                challenges.Add(new Challenge
                {
                    id = c.id,
                    description = c.description,
                    goal = c.goal,
                    currentCount = 0,
                    isCompleted = false,
                    type = c.type
                });
            }

            SaveChallenges();
        }
    }

    [System.Serializable]
    class Wrapper
    {
        public List<Challenge> challenges;
    }
}
