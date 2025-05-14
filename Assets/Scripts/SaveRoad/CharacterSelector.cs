using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public RectTransform[] characterTransforms; // ĳ���� RectTransform �迭
    private int currentIndex = 0;
    public float radius = 300f;
    public float smoothSpeed = 5f;
    public float nonSelectedScale = 0.5f;
    public float selectedScale = 2.0f;

    private Vector2[] targetPositions;

    void Start()
    {
        targetPositions = new Vector2[characterTransforms.Length];
        ArrangeCharacters();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateCharacters(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateCharacters(1);
        }

        // �ε巯�� ��ġ ����
        for (int i = 0; i < characterTransforms.Length; i++)
        {
            characterTransforms[i].anchoredPosition = Vector2.Lerp(
                characterTransforms[i].anchoredPosition,
                targetPositions[i],
                Time.deltaTime * smoothSpeed
            );
        }
    }

    public void RotateLeft()  // UI ���� ��ư���� ȣ��
    {
        RotateCharacters(-1);
    }

    public void RotateRight() // UI ������ ��ư���� ȣ��
    {
        RotateCharacters(1);
    }

    void RotateCharacters(int direction)
    {
        currentIndex += direction;
        if (currentIndex >= characterTransforms.Length) currentIndex = 0;
        if (currentIndex < 0) currentIndex = characterTransforms.Length - 1;

        ArrangeCharacters();
    }

    void ArrangeCharacters()
    {
        for (int i = 0; i < characterTransforms.Length; i++)
        {
            if (i == currentIndex)
            {
                targetPositions[i] = new Vector2(0, 0);
                characterTransforms[i].localScale = Vector3.one * selectedScale;
            }
            else
            {
                if (i == (currentIndex + 1) % characterTransforms.Length)
                {
                    targetPositions[i] = new Vector2(-radius, radius); // �»��
                }
                else if (i == (currentIndex + 2) % characterTransforms.Length)
                {
                    targetPositions[i] = new Vector2(radius, radius);  // ����
                }

                characterTransforms[i].localScale = Vector3.one * nonSelectedScale;
            }
        }
    }
}
