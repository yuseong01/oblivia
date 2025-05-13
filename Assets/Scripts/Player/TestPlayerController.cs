using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    private PlayerStatHandler _playerStatHandler;
    private ItemEffectManager _itemEffectManager;
    private OrbitController _orbitController;

    private Rigidbody2D _rb;
    private Vector2 _movement;

    void Awake()
    {
        _playerStatHandler = GetComponent<PlayerStatHandler>();
        _itemEffectManager = GetComponent<ItemEffectManager>();
        _orbitController = GetComponent<OrbitController>();
        _rb = GetComponent<Rigidbody2D>();

      //  _playerStatHandler.MaxHealth = 6;
   //     _playerStatHandler.Health = 2;

    }

    void Update()
    {
        // WASD 또는 화살표 입력 받기
        _movement.x = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        _movement.y = Input.GetAxisRaw("Vertical");   // -1, 0, 1
        _movement.Normalize(); // 대각선 이동 속도 보정



    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * _playerStatHandler.MoveSpeed * Time.fixedDeltaTime);
    }
}
