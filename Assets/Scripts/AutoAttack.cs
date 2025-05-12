using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public GameObject escMenu;
    public GameObject settingsPanel; //SettingsPanel ������ �ν��Ͻ�
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        escMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        escMenu.SetActive(false);
        settingsPanel.SetActive(false); //ESCMenu ���� �� ����â�� ������
        Time.timeScale = 1f;
        isPaused = false;
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
