using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float deadZone = 0.1f;
    VirtualJoystick joystick;
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
    private void Start()
    {
        joystick = VirtualJoystick.instance.GetComponent<VirtualJoystick>();
    }

    void Update()
    {
        Vector2 input = new Vector2(joystick.horizontal, joystick.vertical);

        float magnitude = Mathf.Min(input.magnitude / joystick.stickRange, 1f);

        if (magnitude < deadZone)
            magnitude = 0f;


        Vector2 ratioInput = input.normalized * magnitude;
        transform.position += (Vector3)(ratioInput * speed * Time.deltaTime);

        if (ratioInput.x != 0)
        {
            _spriteRenderer.flipX = ratioInput.x < 0;
        }
        if (ratioInput != Vector2.zero)
        {
            PlayAnimation("Walk");
        }
        else
        {
           // PlayAnimation("Idle");

            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
            _movement.Normalize();

            // 좌우 방향에 따라 스프라이트 반전
            if (_movement.x != 0)
            {
                _spriteRenderer.flipX = _movement.x < 0;
            }


            // 애니메이션 전환
            if (_movement != Vector2.zero)
            {
                PlayAnimation("Walk");
            }
            else
            {
                PlayAnimation("Idle");
            }
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
