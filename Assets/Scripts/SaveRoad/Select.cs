using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    public GameObject characterSelect; // ĳ���� ���� UI
    public CharacterData[] availableCharacters; // ĳ���� ����Ʈ
    public Button[] characterButtons; // ĳ���� ���� ��ư�� (�����Ϳ��� ����)

    public Image[] characterImages; // ĳ���� ��������Ʈ �̹���

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
            Image spriteImage = characterImages[i]; // ĳ���� ��������Ʈ

            if (i >= availableCharacters.Length)
            {
                characterButtons[i].interactable = false;
                buttonText.text = "???";
                //if (spriteImage != null)
                //{
                //    spriteImage.color = new Color(0.3f, 0.3f, 0.3f, 1f); // ��Ӱ� ǥ��
                //}
                continue;
            }

            string charId = availableCharacters[i].characterId;
            bool isUnlocked = CharacterManager.Instance.IsUnlocked(charId);
            Debug.Log(isUnlocked);

            characterButtons[i].interactable = isUnlocked;
            buttonText.text = isUnlocked ? availableCharacters[i].characterName : "???";

            if (spriteImage != null)
            {
                spriteImage.color = isUnlocked
                    ? Color.white // �رݵ� ĳ���ʹ� ���
                    : new Color(0.3f, 0.3f, 0.3f, 1f); // �رݵ��� ���� ĳ���ʹ� ��Ӱ�
            }

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

    // ��ư�� ���� (�����Ϳ��� ȣ��)
    public void OnTest1ButtonClicked() => SelectCharacter(0);
    public void OnTest2ButtonClicked() => SelectCharacter(1);
    public void OnTest3ButtonClicked() => SelectCharacter(2);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CharacterManager.Instance.UnlockCharacter("2");

            UpdateCharacterButtons();
        }

    }


}
