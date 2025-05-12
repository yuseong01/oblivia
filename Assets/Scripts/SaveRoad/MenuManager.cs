using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject introPanel;
    public GameObject mainMenuPanel;
    public RectTransform panelWrapper;

    [Header("Slide Settings")]
    public float slideDuration = 1f;
    public Vector2 slideOffset = new Vector2(0, -800); // �Ʒ��� �����̵��� �Ÿ�

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

    #region ��Ʈ��
    IEnumerator PlayIntro()
    {
        isIntroPlaying = true;
        introPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

        yield return new WaitForSeconds(2f); // ��Ʈ�� �ð�

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

        panelWrapper.anchoredPosition = targetPos; // ��ġ ����
    }
}
