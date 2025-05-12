using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    public GameObject creat; // �̸� �Է� UI
    public GameObject characterSelect; // ĳ���� ���� UI
    public TextMeshProUGUI[] slotText;
    public TextMeshProUGUI newPlayerName;

    public CharacterData[] availableCharacters; // ĳ���� ����Ʈ
    public Button[] characterButtons; // ĳ���� ���� ��ư�� (�����Ϳ��� ����)

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
            GoGoGame(); // ���� ������ �ٷ� ����
        }
        else
        {
            creat.SetActive(true); // �ű� ������ ��� �̸� �Է�
        }
    }

    public void ConfirmNameAndOpenCharacterSelect()
    {
        string playerName = newPlayerName.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("�÷��̾� �̸��� �Է����ּ���.");
            return;
        }

        creat.SetActive(false);
        characterSelect.SetActive(true);

        // ���� �ؽ�Ʈ�� �����
        foreach (var slot in slotText)
        {
            slot.gameObject.SetActive(false);
        }

        UpdateCharacterButtons(); // ��ư ���� ������Ʈ
    }
    
    // �޴��� ���ƿ� �� �� ���� ���̰��ϴ� �޼���
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
            Debug.LogWarning("��ȿ���� ���� ĳ���� �ε����Դϴ�.");
            return;
        }

        CharacterData selectedCharacter = availableCharacters[characterIndex];

        if (!CharacterManager.Instance.IsUnlocked(selectedCharacter.characterId))
        {
            Debug.LogWarning("�� ĳ���ʹ� ���� �رݵ��� �ʾҽ��ϴ�.");
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

    // ��ư�� ���� (UI���� ���� ����� ��ư��)
    public void OnTest1ButtonClicked() => SelectCharacter(0);
    public void OnTest2ButtonClicked() => SelectCharacter(1);
    public void OnTest3ButtonClicked() => SelectCharacter(2);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CharacterManager.Instance.UnlockCharacter("test3");
            Debug.Log("test3 ĳ���� ���� �رݵ�");
            UpdateCharacterButtons();
        }
    }
}
