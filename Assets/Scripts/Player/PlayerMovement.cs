using System;
using UnityEngine;

enum MovementState
{
    Idle,
    Running,
    Jumping,
    DoubleJumping,
    Falling,
}

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 250f;
        [SerializeField] private float jumpForce = 100f;
        /// <summary>
        /// Maximum slope angle in degrees for the player to be considered grounded.
        /// </summary>
        [SerializeField] private float maxSlope = 45f;
        /// <summary>
        /// Maximum number of jumps in the air (e.g. 2 for double jump).
        /// </summary>
        [SerializeField] private int maxJumpCount = 2;

        private Rigidbody2D _rigidbody2D;
        private ContactFilter2D _contactFilter;
        private bool _wantJump;
        private bool _wasGrounded;
        
        /// <summary>
        /// Times jumped in the air since the player was last grounded. 
        /// </summary>
        public int CurrentJumpCount { get; private set; }
        public float MovementInputX { get; private set; }
        public bool IsGrounded => _rigidbody2D.IsTouching(_contactFilter);
        public event Action OnJump;
        public event Action OnLand;
        
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _contactFilter.SetNormalAngle(maxSlope, maxSlope + 90);
            _contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump")) _wantJump = true;
            
            MovementInputX = Input.GetAxis("Horizontal");
        }
        
        private void FixedUpdate()
        {
            var isGrounded = IsGrounded;
            
            // Reset jump count when landing
            if (isGrounded && !_wasGrounded)
            {
                CurrentJumpCount = 0;
                OnLand?.Invoke();
            };
            
            // Jump
            var canJump = isGrounded || (CurrentJumpCount < maxJumpCount);
            if (_wantJump && canJump)
            {
                _rigidbody2D.linearVelocityY = 0;
                _rigidbody2D.AddForceY(jumpForce);

                CurrentJumpCount++;
                OnJump?.Invoke();
            }
            
            // Apply movement
            //_rigidbody2D.linearVelocityX = MovementInputX * movementSpeed * Time.deltaTime;
            if (MovementInputX != 0)
            {
                _rigidbody2D.linearVelocityX = MovementInputX * movementSpeed * Time.deltaTime;
            }

            _wantJump = false;
            MovementInputX = 0;
            _wasGrounded = isGrounded;
        }
    }
}
