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
    // 프로퍼티 지정 : 외부접근 금지
    public float horizontal {get { return input.x; } }
    public float vertical { get { return input.y; } }

    
    public float stickRange = 100; // 최대 반경
    private Vector3 input;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        transfrom = GetComponent<RectTransform>();
    }

    // 드래그 - 이동
    public void OnDrag(PointerEventData eventData)
    {
        // 스크린 좌표 -> ui의 로컬 좌표
        // out은 드래그 대상 내부 좌표계에서의 위치, 캔버스=overlay 이기 때문에 null 이어도 괜찮음
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transfrom, eventData.position, null, out Vector2 localPoint))
        {
            if (localPoint.magnitude < this.stickRange)
            {
                this.joystickHandle.transform.localPosition = localPoint;
            }
            else   // handleRange의 값 만큼 고정합니다.
            {
                this.joystickHandle.transform.localPosition = localPoint.normalized * this.stickRange;
            }

            this.input = localPoint;
        }
    }

    // 포인터 누르면 드래그 시작
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    // 포인터 놓기 -> 인풋 초기화 및 커서 중앙 이동
    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }
}
