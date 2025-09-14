using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 250f;
        [SerializeField] private float jumpForce = 300f;
        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;
        [SerializeField] private LayerMask groundLayer;
        
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Health _health;
        
        private const float _groundRayDistance = 0.25f;
        private float _moveX;
        private bool _canDoubleJump = true;

        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _health = GetComponent<Health>();

            _health.OnDamage += amount =>
            {
                Debug.Log("Player damaged");
            };
            
            _health.OnDeath += () =>
            {
                Debug.Log("Player dead");
            };
        }

        void Update()
        {
            // Flip sprite towards movement direction
            _moveX = Input.GetAxis("Horizontal");
            if (_moveX != 0)
            {
                _spriteRenderer.flipX = _moveX < 0;
            }

            // Try jump
            var isGrounded = IsGrounded();
            var wantJump = Input.GetButtonDown("Jump");
            // Normal jump
            if (isGrounded && wantJump)
            {
                _animator.SetBool("isDoubleJumping", false);
                _canDoubleJump = true;
                Jump();
            }
            // Double jump
            else if (wantJump && _canDoubleJump)
            {
                _animator.SetBool("isDoubleJumping", true);
                _canDoubleJump = false;
                Jump();
            }
            else
            {
                _animator.SetBool("isDoubleJumping", false);
            }

            // Update animation
            var movementSpeedX = Mathf.Abs(_moveX);
            _animator.SetFloat("movementSpeedX", movementSpeedX);
            _animator.SetBool("isGrounded", isGrounded);
            var movementVelocityY = _rigidbody2D.linearVelocityY;
            _animator.SetFloat("movementVelocityY", movementVelocityY);
        }

        private void Jump()
        {
            _rigidbody2D.linearVelocityY = 0;
            _rigidbody2D.AddForceY(jumpForce);
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
}