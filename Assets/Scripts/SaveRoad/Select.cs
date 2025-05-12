using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    public GameObject characterSelect; // 캐릭터 선택 UI
    public CharacterData[] availableCharacters; // 캐릭터 리스트
    public Button[] characterButtons; // 캐릭터 선택 버튼들 (에디터에서 연결)

    [SerializeField] private CharacterData[] characterData; // 선택 시 넘겨줄 데이터

    void Start()
    {
        DataManager.instance.DataClear();
        UpdateCharacterButtons();        // 캐릭터 버튼 상태 설정
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
            Debug.LogWarning("유효하지 않은 캐릭터 인덱스입니다.");
            return;
        }

        CharacterData selectedCharacter = characterData[characterIndex];

        if (!CharacterManager.Instance.IsUnlocked(selectedCharacter.characterId))
        {
            Debug.LogWarning("이 캐릭터는 아직 해금되지 않았습니다.");
            return;
        }

        DataManager.instance.nowCharacterData = selectedCharacter;
        GoGoGame();
    }

    public void GoGoGame()
    {
        SceneManager.LoadScene("AutoAttack");
    }

    // 테스트용 캐릭터 해금
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CharacterManager.Instance.UnlockCharacter("test3");
            Debug.Log("test3 캐릭터 강제 해금됨");
            UpdateCharacterButtons();
        }
    }

    // 버튼에 연결 (에디터에서 호출)
    public void OnTest1ButtonClicked() => SelectCharacter(0);
    public void OnTest2ButtonClicked() => SelectCharacter(1);
    public void OnTest3ButtonClicked() => SelectCharacter(2);
}
