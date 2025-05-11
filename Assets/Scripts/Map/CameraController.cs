using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public float moveSpeed = 5f;

    private Vector3 targetPos;
    private bool isMoving = false;

    protected override void Awake()
    {
        base.Awake();
        targetPos = transform.position;
    }

    public void MoveRoom(Vector3 newCenter) {
        Debug.Log("카메라 전환");
        targetPos = new Vector3(newCenter.x, newCenter.y, transform.position.z);
        isMoving = true;
    }

    void Update() {
        if (isMoving) {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // 거리 충분히 가까우면 스냅
            if (Vector3.Distance(transform.position, targetPos) < 0.01f) {
                transform.position = targetPos;
                isMoving = false;
            }
        }
    }
}
