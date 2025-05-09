using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class Select : MonoBehaviour
{
    public GameObject creat;
    public TextMeshProUGUI[] slotText;
    public TextMeshProUGUI newPlayerName;

    bool[] savefile = new bool[3];
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< 3; i++ )
        {
            if (File.Exists(DataManager.instance.path +$"{i}"))
            {
                savefile[i] = true;
                DataManager.instance.nowSlot = i;
                DataManager.instance.LoadData();
                slotText[i].text = DataManager.instance.nowPlayer.name;
            }
            else
            {
                slotText[i].text = "Empty";
            }
        }

        DataManager.instance.DataClear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Slot(int number)
    {
        DataManager.instance.nowSlot = number;

        if(savefile[number])
        {
            DataManager.instance.LoadData();
            GoGoGame();
        }
        else
        {
            Creat();
        }

    }

    public void Creat()
    {
        creat.gameObject.SetActive(true);
    }
    
    public void GoGoGame()
    {
        if (!savefile[DataManager.instance.nowSlot])
        {
            DataManager.instance.nowPlayer.name = newPlayerName.text;
            DataManager.instance.SaveData();
        }
        SceneManager.LoadScene("Test");
    }
}
