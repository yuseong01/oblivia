using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour
{
    public GameObject creat; // �̸� �Է� UI
    public GameObject characterSelect; // ĳ���� ���� UI
    public TextMeshProUGUI[] slotText;
    public TextMeshProUGUI newPlayerName;

    public CharacterData[] availableCharacters; // ĳ���� ����Ʈ
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
    }

    public void SelectCharacter(int characterIndex)
    {
        if (characterIndex < 0 || characterIndex >= availableCharacters.Length)
        {
            Debug.LogWarning("��ȿ���� ���� ĳ���� �ε����Դϴ�.");
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

        GoGoGame(); // ĳ���� ���� �� �� ��ȯ
    }

    public void GoGoGame()
    {
        SceneManager.LoadScene("Test");
    }

    // ��ư�� ����
    public void OnTest1ButtonClicked() => SelectCharacter(0);
    public void OnTest2ButtonClicked() => SelectCharacter(1);
    public void OnTest3ButtonClicked() => SelectCharacter(2);
}
