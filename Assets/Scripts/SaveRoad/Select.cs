using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour
{
    public GameObject creat; // 이름 입력 UI
    public GameObject characterSelect; // 캐릭터 선택 UI
    public TextMeshProUGUI[] slotText;
    public TextMeshProUGUI newPlayerName;

    public CharacterData[] availableCharacters; // 캐릭터 리스트
    private bool[] savefile = new bool[3];

    private int currentSlot = -1;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            string filePath = DataManager.instance.path + $"{i}.json";

            if (File.Exists(filePath))
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
        currentSlot = number;
        DataManager.instance.nowSlot = number;

        if (savefile[number])
        {
            DataManager.instance.LoadData();
            GoGoGame(); // 기존 슬롯은 바로 시작
        }
        else
        {
            creat.SetActive(true); // 신규 유저일 경우 이름 입력
        }
    }

    public void ConfirmNameAndOpenCharacterSelect()
    {
        string playerName = newPlayerName.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("플레이어 이름을 입력해주세요.");
            return;
        }

        creat.SetActive(false);
        characterSelect.SetActive(true);
    }

    public void SelectCharacter(int characterIndex)
    {
        if (characterIndex < 0 || characterIndex >= availableCharacters.Length)
        {
            Debug.LogWarning("유효하지 않은 캐릭터 인덱스입니다.");
            return;
        }

        CharacterData selectedCharacter = availableCharacters[characterIndex];

        DataManager.instance.nowPlayer.playerName = newPlayerName.text.Trim();
        DataManager.instance.nowPlayer.characterName = selectedCharacter.characterName;
        DataManager.instance.nowPlayer._damage = selectedCharacter.damage;
        DataManager.instance.nowPlayer._attackRate = selectedCharacter.attackRate;
        DataManager.instance.nowPlayer._attackDelay = selectedCharacter.attackDelay;
        DataManager.instance.nowPlayer._attackSpeed = selectedCharacter.attackSpeed;
        DataManager.instance.nowPlayer._attackRangef = selectedCharacter.attackRange;

        DataManager.instance.SaveData();

        GoGoGame(); // 캐릭터 선택 후 씬 전환
    }

    public void GoGoGame()
    {
        SceneManager.LoadScene("Test");
    }

    // 버튼에 연결
    public void OnTest1ButtonClicked() => SelectCharacter(0);
    public void OnTest2ButtonClicked() => SelectCharacter(1);
    public void OnTest3ButtonClicked() => SelectCharacter(2);
}
