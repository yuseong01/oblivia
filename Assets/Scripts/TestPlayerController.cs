using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // WASD 또는 화살표 입력 받기
        movement.x = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        movement.y = Input.GetAxisRaw("Vertical");   // -1, 0, 1
        movement.Normalize(); // 대각선 이동 속도 보정
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
