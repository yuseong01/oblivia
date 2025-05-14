using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button closeButton; 

    public bool isOpen => settingsPanel.activeSelf;

    private void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SoundManager.Instance.SetBGMVolume(bgmSlider.value);
        SoundManager.Instance.SetSFXVolume(sfxSlider.value);

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        //settingsPanel.SetActive(false); // ������ �� ��Ȱ��ȭ

        closeButton.onClick.AddListener(CloseSettings);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen)
                CloseSettings();
            else
                OpenSettings();
        }
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        // Time.timeScale = 0f;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        // Time.timeScale = 1f;
    }

    private void SetBGMVolume(float volume)
    {
        SoundManager.Instance.SetBGMVolume(volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    private void SetSFXVolume(float volume)
    {
        SoundManager.Instance.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
