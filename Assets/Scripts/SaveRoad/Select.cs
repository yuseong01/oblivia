using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour
{
    public GameObject creat;
    public GameObject characterSelect;
    public TextMeshProUGUI[] slotText;
    public TextMeshProUGUI newPlayerName;

    public CharacterData[] availableCharacters; // 캐릭터 리스트
    bool[] savefile = new bool[3];

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.instance.path + $"{i}.json"))
            {
                savefile[i] = true;
                DataManager.instance.nowSlot = i;
                DataManager.instance.LoadData();
                slotText[i].text = DataManager.instance.nowPlayer.playerName;
            }
            else
            {
                slotText[i].text = "Empty";
            }
        }

        DataManager.instance.DataClear();
    }

    public void Slot(int number)
    {
        DataManager.instance.nowSlot = number;

        if (savefile[number])
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
        creat.SetActive(true);
    }

    public void CharcterSelect()
    {
        creat.SetActive(false);
        characterSelect.SetActive(true);
    }

    public void GoGoGame()
    {
        SceneManager.LoadScene("Test");
    }

    public void SelectCharacter(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < availableCharacters.Length)
        {
            CharacterData selectedCharacter = availableCharacters[characterIndex];

            DataManager.instance.nowPlayer.playerName = newPlayerName.text;
            DataManager.instance.nowPlayer.characterName = selectedCharacter.characterName;
            DataManager.instance.nowPlayer._damage = selectedCharacter.damage;
            DataManager.instance.nowPlayer._attackRate = selectedCharacter.attackRate;
            DataManager.instance.nowPlayer._attackDelay = selectedCharacter.attackDelay;
            DataManager.instance.nowPlayer._attackSpeed = selectedCharacter.attackSpeed;
            DataManager.instance.nowPlayer._attackRangef = selectedCharacter.attackRange;

            DataManager.instance.SaveData();
        }

        GoGoGame();
    }

    public void OnTest1ButtonClicked() => SelectCharacter(0);
    public void OnTest2ButtonClicked() => SelectCharacter(1);
    public void OnTest3ButtonClicked() => SelectCharacter(2);
}
