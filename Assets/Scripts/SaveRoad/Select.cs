using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    public GameObject creat; // 이름 입력 UI
    public GameObject characterSelect; // 캐릭터 선택 UI
    public TextMeshProUGUI[] slotText;
    public TextMeshProUGUI newPlayerName;

    public CharacterData[] availableCharacters; // 캐릭터 리스트
    public Button[] characterButtons; // 캐릭터 선택 버튼들 (에디터에서 연결)

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

        // 슬롯 텍스트들 숨기기
        foreach (var slot in slotText)
        {
            slot.gameObject.SetActive(false);
        }

        UpdateCharacterButtons(); // 버튼 상태 업데이트
    }
    
    // 메뉴로 돌아올 때 쓸 슬롯 보이게하는 메서드
    public void BackToSlotSelect()
    {
        characterSelect.SetActive(false);
        creat.SetActive(false);

        foreach (var slot in slotText)
        {
            slot.gameObject.SetActive(true);
        }
    }

    private void UpdateCharacterButtons()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = characterButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            if (i >= availableCharacters.Length)
            {
                characterButtons[i].interactable = false;
                buttonText.text = "???";
                continue;
            }

            string charId = availableCharacters[i].characterId;
            bool isUnlocked = CharacterManager.Instance.IsUnlocked(charId);

            characterButtons[i].interactable = isUnlocked;
            buttonText.text = isUnlocked ? availableCharacters[i].characterName : "???";
        }
    }

    public void SelectCharacter(int characterIndex)
    {
        if (characterIndex < 0 || characterIndex >= availableCharacters.Length)
        {
            Debug.LogWarning("유효하지 않은 캐릭터 인덱스입니다.");
            return;
        }

        CharacterData selectedCharacter = availableCharacters[characterIndex];

        if (!CharacterManager.Instance.IsUnlocked(selectedCharacter.characterId))
        {
            Debug.LogWarning("이 캐릭터는 아직 해금되지 않았습니다.");
            return;
        }

        DataManager.instance.nowPlayer.playerName = newPlayerName.text.Trim();

        DataManager.instance.nowPlayer.characterName = selectedCharacter.characterName;
        DataManager.instance.nowPlayer.characterId = selectedCharacter.characterId;

        DataManager.instance.nowPlayer.moveSpeed = selectedCharacter.moveSpeed;
        DataManager.instance.nowPlayer.maxHealth = selectedCharacter.maxHealth;

        DataManager.instance.nowPlayer.damage = selectedCharacter.damage;
        DataManager.instance.nowPlayer.attackRate = selectedCharacter.attackRate;
        DataManager.instance.nowPlayer.attackDelay = selectedCharacter.attackDelay;
        DataManager.instance.nowPlayer.attackSpeed = selectedCharacter.attackSpeed;
        DataManager.instance.nowPlayer.attackRange = selectedCharacter.attackRange;
        DataManager.instance.nowPlayer.attackCount = selectedCharacter.attackCount;
        DataManager.instance.nowPlayer.attackAngle = selectedCharacter.attackAngle;
        DataManager.instance.nowPlayer.knockbackForce = selectedCharacter.knockbackForce;
        DataManager.instance.nowPlayer.attackDuration = selectedCharacter.attackDuration;
        DataManager.instance.nowPlayer.projectileSize = selectedCharacter.projectileSize;

        DataManager.instance.SaveData();

        GoGoGame();
    }

    public void GoGoGame()
    {
        SceneManager.LoadScene("Test");
    }

    // 버튼에 연결 (UI에서 직접 연결된 버튼들)
    public void OnTest1ButtonClicked() => SelectCharacter(0);
    public void OnTest2ButtonClicked() => SelectCharacter(1);
    public void OnTest3ButtonClicked() => SelectCharacter(2);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CharacterManager.Instance.UnlockCharacter("test3");
            Debug.Log("test3 캐릭터 강제 해금됨");
            UpdateCharacterButtons();
        }
    }
}
