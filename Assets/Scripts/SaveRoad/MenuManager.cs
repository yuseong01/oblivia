using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject introPanel;
    public GameObject mainMenuPanel;
    public RectTransform panelWrapper;
    public GameObject introText;

    [Header("Slide Settings")]
    public float slideDuration = 1f;
    public Vector2 slideOffset = new Vector2(0, -800); // 아래로 슬라이드할 거리

    private Coroutine introCoroutine;
    private bool isIntroPlaying = false;

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

    #region 인트로
    IEnumerator PlayIntro()
    {
        isIntroPlaying = true;
        introPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

        // 인트로 이미지 표시 후 41초간 대기 총 인트로 시간은 43초
        yield return new WaitForSeconds(2f);

        // 이미지 비활성화
        if (introText != null)
        {
            introText.SetActive(false);
        }

        // 2초 더 대기 후 인트로 종료
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

        // 위로 되돌리기 위해 양수로 이동
        Vector2 dynamicOffset = new Vector2(0, panelWrapper.rect.height);
        Vector2 targetPos = startPos + dynamicOffset;

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

    IEnumerator SlideToMainMenu()
    {
        Vector2 startPos = panelWrapper.anchoredPosition;

        // 현재 패널의 높이만큼 아래로 슬라이드
        Vector2 dynamicOffset = new Vector2(0, -panelWrapper.rect.height);
        Vector2 targetPos = startPos + dynamicOffset;

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
