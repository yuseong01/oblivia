using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// �����ϴ� ���
// 1. ������ �����Ͱ� ����
// 2. �����͸� Json���� ��ȯ
// 3. Json�� �ܺο� ����

// �ҷ����� ���
// 1. �ܺο� ����� Json�� ������
// 2. Json�� ������ ���·� ��ȯ
// 3. �ҷ��� �����͸� ���

public class PlayerData
{
    // ���忡 �ʿ��� ������ �߰�
    public string name;
    public float _damage =3f;
    public float _attackRate =3f;
    public float _attackDelay = 3f;
    public float _attackSpeed = 3f;
    public float _attackRange = 3f;
}

public class DataManager : MonoBehaviour
{
    //�̱���
    public static DataManager instance;

    public PlayerData nowPlayer = new PlayerData();

    public GameObject player;

    public string path;
    public int nowSlot;

    private void Awake()
    {
        #region �̱���
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
        // ���� ���� �� ���� ���� �ڵ�
    }
}
