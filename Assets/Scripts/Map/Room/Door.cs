using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Door : MonoBehaviour
{
    public Direction direction;
    public bool isNeighbor = false;
    //public Transform transform;

    void Awake() {
        SetDir();
    }

    public void SetDoor() {
        gameObject.SetActive(true);
    }

    void DoorOpen() {
        // 몬스터 모두 처치 시 문 오픈
        // 아마 추후 애니메이션 처리할 듯
        gameObject.SetActive(true);
    }

    public void DoorClose() {
        // 방에 들어갔을 때, 문 클로즈
        // 아마 추후 애니메이션 처리할 듯
        gameObject.SetActive(false);
    }

    void SetDir()
    {
        Vector2 pos = transform.localPosition;

        if (Mathf.Abs(pos.x) > Mathf.Abs(pos.y)) {
            direction = (pos.x > 0) ? Direction.Right : Direction.Left;
        }
        else {
            direction = (pos.y > 0) ? Direction.Up : Direction.Down;
        }
    }

}
