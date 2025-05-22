using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : Singleton<MenuManager>
{
    
    
    [Header("UI Panels")]
    public GameObject introPanel;
    public GameObject mainMenuPanel;
    public RectTransform panelWrapper;
    public GameObject introText;

    [Header("Slide Settings")]
    public float slideDuration = 1f;
    public Vector2 slideOffset = new Vector2(0, -800);

    private Coroutine introCoroutine;
    private bool isIntroPlaying = false;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        introCoroutine = StartCoroutine(PlayIntro());
    }

    private void Update()
    {
        if (isIntroPlaying && Input.anyKeyDown)
        {
            SkipIntro();
        }
    }

    #region ��Ʈ��
    IEnumerator PlayIntro()
    {
        isIntroPlaying = true;
        introPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

        // ��Ʈ�� �̹��� ǥ�� �� 41�ʰ� ��� �� ��Ʈ�� �ð��� 43��
        yield return new WaitForSeconds(2f);

        // �̹��� ��Ȱ��ȭ
        if (introText != null)
        {
            introText.SetActive(false);
        }

        // 2�� �� ��� �� ��Ʈ�� ����
        yield return new WaitForSeconds(41f);

        EndIntro();
    }
    void SkipIntro()
    {
        if (introCoroutine != null)
        {
            StopCoroutine(introCoroutine);
            introCoroutine = null;
        }
        EndIntro();
    }

    void EndIntro()
    {
        isIntroPlaying = false;
        introPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        Debug.Log(SoundManager.Instance.DefaultBGMClip);
        SoundManager.Instance.PlayBGMSource(SoundManager.Instance.DefaultBGMClip);
    }

    #endregion

    public void OnStartGameButton()
    {
        StartCoroutine(SlideToCharacterSelect());
    }

    IEnumerator SlideToCharacterSelect()
    {
        Vector2 startPos = panelWrapper.anchoredPosition;
        Vector2 targetPos = startPos + slideOffset;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            panelWrapper.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / slideDuration);
            yield return null;
        }

        panelWrapper.anchoredPosition = targetPos;
    }

    public void OnBackToMainMenuButton()
    {
        StartCoroutine(SlideToMainMenu());
    }

    private IEnumerator SlideToMainMenu()
    {
        Vector2 startPos = panelWrapper.anchoredPosition;
        Vector2 targetPos = startPos - slideOffset;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            panelWrapper.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / slideDuration);
            yield return null;
        }

        panelWrapper.anchoredPosition = targetPos;
    }
}
