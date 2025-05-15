using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public static VirtualJoystick instance;

    private RectTransform transfrom;
    public RectTransform joystickFrame;
    public RectTransform joystickHandle;
    // ������Ƽ ���� : �ܺ����� ����
    public float horizontal {get { return input.x; } }
    public float vertical { get { return input.y; } }

    
    public float stickRange = 100; // �ִ� �ݰ�
    private Vector3 input;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        transfrom = GetComponent<RectTransform>();
    }

    // �巡�� - �̵�
    public void OnDrag(PointerEventData eventData)
    {
        // ��ũ�� ��ǥ -> ui�� ���� ��ǥ
        // out�� �巡�� ��� ���� ��ǥ�迡���� ��ġ, ĵ����=overlay �̱� ������ null �̾ ������
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transfrom, eventData.position, null, out Vector2 localPoint))
        {
            if (localPoint.magnitude < this.stickRange)
            {
                this.joystickHandle.transform.localPosition = localPoint;
            }
            else   // handleRange�� �� ��ŭ �����մϴ�.
            {
                this.joystickHandle.transform.localPosition = localPoint.normalized * this.stickRange;
            }

            this.input = localPoint;
        }
    }

    // ������ ������ �巡�� ����
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    // ������ ���� -> ��ǲ �ʱ�ȭ �� Ŀ�� �߾� �̵�
    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }
}
