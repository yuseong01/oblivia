using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    public GameObject characterSelect; // ĳ���� ���� UI
    public CharacterData[] availableCharacters; // ĳ���� ����Ʈ
    public Button[] characterButtons; // ĳ���� ���� ��ư�� (�����Ϳ��� ����)

    [SerializeField] private CharacterData[] characterData; // ���� �� �Ѱ��� ������

    void Start()
    {
        DataManager.instance.DataClear();
        UpdateCharacterButtons();        // ĳ���� ��ư ���� ����
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
        if (characterIndex < 0 || characterIndex >= characterData.Length)
        {
            Debug.LogWarning("��ȿ���� ���� ĳ���� �ε����Դϴ�.");
            return;
        }

        CharacterData selectedCharacter = characterData[characterIndex];

        if (!CharacterManager.Instance.IsUnlocked(selectedCharacter.characterId))
        {
            Debug.LogWarning("�� ĳ���ʹ� ���� �رݵ��� �ʾҽ��ϴ�.");
            return;
        }

        DataManager.instance.nowCharacterData = selectedCharacter;
        GoGoGame();
    }

    public void GoGoGame()
    {
        SceneManager.LoadScene("AutoAttack");
    }

    // �׽�Ʈ�� ĳ���� �ر�
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CharacterManager.Instance.UnlockCharacter("test3");
            Debug.Log("test3 ĳ���� ���� �رݵ�");
            UpdateCharacterButtons();
        }
    }

    // ��ư�� ���� (�����Ϳ��� ȣ��)
    public void OnTest1ButtonClicked() => SelectCharacter(0);
    public void OnTest2ButtonClicked() => SelectCharacter(1);
    public void OnTest3ButtonClicked() => SelectCharacter(2);
}
