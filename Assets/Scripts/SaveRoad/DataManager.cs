using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

// 저장하는 방법
// 1. 저장할 데이터가 존재
// 2. 데이터를 Json으로 변환
// 3. Json을 외부에 저장

// 불러오는 방법
// 1. 외부에 저장된 Json을 가져옴
// 2. Json을 데이터 형태로 변환
// 3. 불러온 데이터를 사용

public class PlayerData
{
    public string characterName;
    public string characterId;

    public float moveSpeed;
    public float maxHealth;

    public float damage;
    public float attackRate;
    public float attackDelay;
    public float attackSpeed;
    public float attackRange;
    public int attackCount;
    public float attackAngle;
    public float knockbackForce;
    public float attackDuration;
    public float projectileSize;
}

public class DataManager : MonoBehaviour
{
    public CharacterData nowCharacterData;

    public static DataManager instance;

    public PlayerData nowPlayer = new PlayerData();

    public string path;
    public int nowSlot;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);

        path = Application.persistentDataPath + "/save";
        Debug.Log("Save Path: " + path);

       

    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(nowPlayer);
        File.WriteAllText(path + nowSlot.ToString() + ".json", data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + nowSlot.ToString() + ".json");
        nowPlayer = JsonUtility.FromJson<PlayerData>(data);
    }

    public void DataClear()
    {
        nowSlot = -1;
        nowPlayer = new PlayerData();
    }

    public void DeleteData()
    {
        // 추후 구현
    }

    public void SetCharter()
    {
        var Go = GameObject.FindWithTag("Player");
        var StatHandler = Go.GetComponent<PlayerStatHandler>();

        //DataManager.instance.nowPlayer.characterName = selectedCharacter.characterName;
        //DataManager.instance.nowPlayer.characterId = selectedCharacter.characterId;

        StatHandler.MoveSpeed = nowCharacterData.moveSpeed;
        StatHandler.MaxHealth = nowCharacterData.maxHealth;
        StatHandler.Health = nowCharacterData.maxHealth;
        StatHandler.Damage = nowCharacterData.damage;
        StatHandler.AttackRate = nowCharacterData.attackRate;
        StatHandler.AttackDelay = nowCharacterData.attackDelay;
        StatHandler.AttackSpeed = nowCharacterData.attackSpeed;
        StatHandler.AttackRange = nowCharacterData.attackRange;
        StatHandler.AttackCount = nowCharacterData.attackCount;
        StatHandler.AttackAngle = nowCharacterData.attackAngle;
        StatHandler.KnockbackForce = nowCharacterData.knockbackForce;
        StatHandler.AttackDuration = nowCharacterData.attackDuration;
        StatHandler.ProjectileSize = nowCharacterData.projectileSize;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "AutoAttack")
        {
            SetCharter();
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}


