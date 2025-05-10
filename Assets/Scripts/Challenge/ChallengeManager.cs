using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class ChallengeManager : MonoBehaviour
{
    public ChallengeDatabase database;
    public GameObject rewardPanel;
    public TextMeshProUGUI rewardText;

    private List<Challenge> challenges = new List<Challenge>();
    private string savePath;

    void Awake()
    {
        savePath = Application.persistentDataPath + "/challenges.json";
        LoadChallenges();
    }

    public void IncreaseProgress(string id, int amount)
    {
        foreach (Challenge challenge in challenges)
        {
            if (challenge.id == id && !challenge.isCompleted)
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

    void ShowReward(Challenge challenge)
    {
        rewardText.text = $"Misson Complete!\n{challenge.description}";
        rewardPanel.SetActive(true);
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
                    isCompleted = false
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
