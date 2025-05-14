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

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    private string _currentAnim = "";

    void Awake()
    {
        _playerStatHandler = GetComponent<PlayerStatHandler>();
        _itemEffectManager = GetComponent<ItemEffectManager>();
        _orbitController = GetComponent<OrbitController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        _movement.Normalize();

        // �¿� ���⿡ ���� ��������Ʈ ����
        if (_movement.x != 0)
        {
            _spriteRenderer.flipX = _movement.x < 0;
        }


        // �ִϸ��̼� ��ȯ
        if (_movement != Vector2.zero)
        {
            PlayAnimation("Walk");
        }
        else
        {
            PlayAnimation("Idle");
        }
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * _playerStatHandler.MoveSpeed * Time.fixedDeltaTime);
    }

    private void PlayAnimation(string animationName)
    {
        if (_currentAnim == animationName) return;
        _currentAnim = animationName;
        _animator.CrossFade(animationName, 0f);
    }
}
