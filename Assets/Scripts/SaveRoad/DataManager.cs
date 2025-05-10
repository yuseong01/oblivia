using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    // 저장에 필요한 데이터 추가
    public string playerName;
    public string characterName;
    public float _damage;
    public float _attackRate;
    public float _attackDelay;
    public float _attackSpeed;
    public float _attackRangef;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public PlayerData nowPlayer = new PlayerData();
    public GameObject player;
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
}


