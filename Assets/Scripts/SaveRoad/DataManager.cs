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
    public string name;
    public float _damage =3f;
    public float _attackRate =3f;
    public float _attackDelay = 3f;
    public float _attackSpeed = 3f;
    public float _attackRange = 3f;
}

public class DataManager : MonoBehaviour
{
    //싱글톤
    public static DataManager instance;

    public PlayerData nowPlayer = new PlayerData();

    public GameObject player;

    public string path;
    public int nowSlot;

    private void Awake()
    {
        #region 싱글톤
        if (instance == null) instance = this;
        else if (instance != null) Destroy(instance.gameObject);
        DontDestroyOnLoad(this.gameObject);
        #endregion
        
        path = Application.persistentDataPath + "/save";
        print(path);
    }

    private void Start()
    {

    }

    public void SaveData()
    {
        
        string data = JsonUtility.ToJson(nowPlayer);
        File.WriteAllText(path +  nowSlot.ToString(), data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + nowSlot.ToString());
        nowPlayer = JsonUtility.FromJson<PlayerData>(data);
    }

    public void DataClear()
    {
        nowSlot = -1;
        nowPlayer = new PlayerData();
    }

    public void DeleteData()
    {
        // 게임 종료 시 파일 삭제 코드
    }
}
