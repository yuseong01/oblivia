using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TestScene : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI attackRate;
    // Start is called before the first frame update
    void Start()
    {
        name.text += DataManager.instance.nowPlayer.playerName;
        damage.text += DataManager.instance.nowPlayer.damage.ToString();
        attackRate.text += DataManager.instance.nowPlayer.attackRate.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageUp()
    {
        DataManager.instance.nowPlayer.damage++;
        damage.text = "Damage : " + DataManager.instance.nowPlayer.damage.ToString();
    }

    public void AttackRateUp()
    {
        DataManager.instance.nowPlayer.attackRate++;
        attackRate.text= "Attack Rate : "+ DataManager.instance.nowPlayer.attackRate.ToString();
    }

    public void Save()
    {
        DataManager.instance.SaveData();
    }
}
