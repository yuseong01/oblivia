using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI attackRate;
    // Start is called before the first frame update
    void Start()
    {
        name.text += DataManager.instance.nowPlayer.playerName;
        damage.text += DataManager.instance.nowPlayer._damage.ToString();
        attackRate.text += DataManager.instance.nowPlayer._attackRate.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageUp()
    {
        DataManager.instance.nowPlayer._damage++;
        damage.text = "Damage : " + DataManager.instance.nowPlayer._damage.ToString();
    }

    public void AttackRateUp()
    {
        DataManager.instance.nowPlayer._attackRate++;
        attackRate.text= "Attack Rate : "+ DataManager.instance.nowPlayer._attackRate.ToString();
    }

    public void Save()
    {
        DataManager.instance.SaveData();
    }
}
