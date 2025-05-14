using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public GameObject escMenu;
    public GameObject itemMenu;
    public GameObject settingsPanel; // SettingsPanel 프리팹 인스턴스
    private bool isPausedGame = false;
    private bool isPausedItem = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            TogglePauseItem();
        }

    }

    public void TogglePauseGame()
    {
        if (isPausedGame)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        escMenu.SetActive(true);
        Time.timeScale = 0f;
        isPausedGame = true;
    }

    public void ResumeGame()
    {
        escMenu.SetActive(false);
        settingsPanel.SetActive(false); // ESCMenu 꺼질 때 세팅창도 꺼지기
        Time.timeScale = 1f;
        isPausedGame = false;
    }

    public void TogglePauseItem()
    {
        if (isPausedItem)
            CloseItem();
        else
            ShowItem();
    }
    public void ShowItem()
    {
        itemMenu.SetActive(true);
        Time.timeScale = 0f;
        isPausedItem= true;
    }

    public void CloseItem()
    {
        itemMenu.SetActive(false) ;
        settingsPanel.SetActive(false) ;
        Time.timeScale = 1f;
        isPausedItem = false;
    }
    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
