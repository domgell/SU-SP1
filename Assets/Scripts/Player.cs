using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 250f;
    [SerializeField] private float jumpForce = 300f;

    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;
    [SerializeField] private LayerMask groundLayer;

    private const float _groundRayDistance = 0.25f;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private float _moveX;
    
    private bool canDoubleJump;
    private bool doubleJumpedLastFrame;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (doubleJumpedLastFrame)
        {
            _animator.SetBool("isDoubleJumping", false);
            doubleJumpedLastFrame = false;
        }
        
        // Flip sprite towards movement direction
        _moveX = Input.GetAxis("Horizontal");
        if (_moveX != 0)
        {
            _spriteRenderer.flipX = _moveX < 0;
        }

        // Try jump
        var isGrounded = IsGrounded();
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            _rigidbody2D.AddForceY(jumpForce);
        }

        // Update animation
        var movementSpeedX = Mathf.Abs(_moveX);
        _animator.SetFloat("movementSpeedX", movementSpeedX);
        _animator.SetBool("isGrounded", isGrounded);
        var movementVelocityY = _rigidbody2D.linearVelocityY;
        _animator.SetFloat("movementVelocityY", movementVelocityY);
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocityX = _moveX * movementSpeed * Time.deltaTime;
    }

    private bool IsGrounded()
    {
        var left = Physics2D.Raycast(leftFoot.position, Vector2.down, _groundRayDistance, groundLayer);
        var right = Physics2D.Raycast(rightFoot.position, Vector2.down, _groundRayDistance, groundLayer);
        return left || right;
    }
}