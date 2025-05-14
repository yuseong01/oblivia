using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    private HashSet<string> unlockedCharacterIds = new HashSet<string>();  // 해금된 캐릭터 ID 목록
    private string savePath;

    // 기본 해금 캐릭터 ID 목록
    private readonly string[] defaultUnlockedIds = { "1", "3" };

    [System.Serializable]
    private class UnlockSaveData
    {
        public List<string> unlockedIds = new List<string>();
    }

    private void Awake()
    {
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

        savePath = Path.Combine(Application.persistentDataPath, "unlocked_characters.json");
        LoadUnlockData();
    }

    private void LoadUnlockData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            var data = JsonUtility.FromJson<UnlockSaveData>(json);
            unlockedCharacterIds = new HashSet<string>(data.unlockedIds);
        }

        // 기본 해금 캐릭터 보장
        foreach (var id in defaultUnlockedIds)
        {
            if (!unlockedCharacterIds.Contains(id))
            {
                unlockedCharacterIds.Add(id);
            }
        }

        SaveUnlockData(); // 기본 캐릭터도 저장에 반영
    }

    private void SaveUnlockData()
    {
        var data = new UnlockSaveData
        {
            unlockedIds = new List<string>(unlockedCharacterIds)
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public bool IsUnlocked(string characterId)
    {
        return unlockedCharacterIds.Contains(characterId);
    }

    public void UnlockCharacter(string characterId)
    {
        if (!unlockedCharacterIds.Contains(characterId))
        {
            unlockedCharacterIds.Add(characterId);
            SaveUnlockData();
            Debug.Log($"캐릭터 해금됨: {characterId}");
        }
    }

}
