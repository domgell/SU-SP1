using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public enum CollisionPlane
        {
            None,
            Ground,
            LeftWall,
            RightWall,
        }

        [Header("Movement")] [Space] [SerializeField]
        private float movementSpeed = 10f;

        [SerializeField] private float movementAcceleration = 10f;
        [SerializeField] private float movementDeceleration = 5f;

        [Header("Jumping")] [Space] [SerializeField]
        private float jumpForce = 400f;

        /// <summary>
        /// Multiplier for the horizontal force applied when performing a wall jump.
        /// `Sideways force = jumpForce * wallJumpForceMultiplier`
        /// </summary>
        [SerializeField] private float wallJumpForceMultiplier = 0.75f;

        /// <summary>
        /// Amount of time to ignore player input after a wall jump.
        /// Prevents the player from going back into the wall immediately after wall jump.
        /// </summary>
        [FormerlySerializedAs("wallJumpTimeIgnoreInputTime")] [SerializeField] private float wallJumpIgnoreInputTime = 0.25f;

        /// <summary>
        /// Maximum slope angle in degrees for the player to be considered grounded.
        /// </summary>
        [FormerlySerializedAs("maxSlope")] [SerializeField] private float maxSlopeDegrees = 45f;

        /// <summary>
        /// Maximum number of jumps in the air (e.g. 2 for double jump).
        /// </summary>
        [SerializeField] private int maxJumpCount = 2;

        /// <summary>
        /// Downward force applied when sliding down a wall.
        /// </summary>
        [SerializeField] private float wallSlideDownForce = 10f;

        private Rigidbody2D _rigidbody2D;
        private ContactFilter2D _groundFilter;
        private ContactFilter2D _leftWallFilter;
        private ContactFilter2D _rightWallFilter;
        private bool _wantJump;
        private bool _wantMove;
        private bool _wasGrounded;
        private bool _wasOnWall;
        private float _timeSinceWallJump;
        private CollisionPlane _prevCollisionPlane;

        /// <summary>
        /// Times jumped in the air since the player was last grounded. 
        /// </summary>
        [HideInInspector]
        public int CurrentJumpCount { get; private set; }

        /// <summary>
        /// Raw horizontal movement input in the range [-1, 1].
        /// </summary>
        [HideInInspector]
        public float MovementInputX { get; private set; }

        /// <summary>
        /// The current collision plane the player is touching.
        /// </summary>
        [HideInInspector]
        public CollisionPlane CurrentCollisionPlane { get; private set; }
        
        /// <summary>
        /// Invoked when the player jumps (from ground, wall or air)
        /// </summary>
        public event Action OnJump;

        /// <summary>
        /// Invoked when the player lands on the ground or hits a wall
        /// </summary>
        public event Action OnLand;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            var layerMask = LayerMask.GetMask("Ground");
            _groundFilter.SetNormalAngle(maxSlopeDegrees, maxSlopeDegrees + 90);
            _groundFilter.SetLayerMask(layerMask);

            _leftWallFilter.SetNormalAngle(90 + maxSlopeDegrees, 270 - maxSlopeDegrees);
            _leftWallFilter.SetLayerMask(layerMask);
            _rightWallFilter.SetNormalAngle(270 + maxSlopeDegrees, 450 - maxSlopeDegrees);
            _rightWallFilter.SetLayerMask(layerMask);
        }

        private void Update()
        {
            MovementInputX = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f)
            {
                _wantMove = true;
            }

            if (Input.GetButtonDown("Jump"))
            {
                _wantJump = true;
            }

            _timeSinceWallJump += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            CurrentCollisionPlane = GetCollisionPlane();

            // Slide down wall
            if (CurrentCollisionPlane is CollisionPlane.LeftWall or CollisionPlane.RightWall)
            {
                _rigidbody2D.AddForceY(-wallSlideDownForce);
            }

            UpdateMovement();
            UpdateJump();

            _wantJump = false;
            _wantMove = false;
            _prevCollisionPlane = CurrentCollisionPlane;
        }

        private void UpdateMovement()
        {
            var targetSpeed = MovementInputX * movementSpeed;
            var currentVelocityX = _rigidbody2D.linearVelocityX;

            // Acceleration
            if (_wantMove)
            {
                if (Mathf.Abs(currentVelocityX) < Mathf.Abs(targetSpeed)
                    && _timeSinceWallJump > wallJumpIgnoreInputTime)
                {
                    var targetSpeedDifference = targetSpeed - currentVelocityX;
                    var acceleration = targetSpeedDifference * movementAcceleration;
                    _rigidbody2D.AddForceX(acceleration);
                }
            }
            // Deceleration
            else
            {
                var deceleration = -currentVelocityX * movementDeceleration;
                _rigidbody2D.AddForceX(deceleration);
            }
        }

        private void UpdateJump()
        {
            // Reset jump count upon hitting ground/wall
            if (CurrentCollisionPlane is not CollisionPlane.None && _prevCollisionPlane is CollisionPlane.None)
            {
                OnLand?.Invoke();
                CurrentJumpCount = 0;

                // Reset Y velocity when landing on wall to prevent slipping off immediately
                if (CurrentCollisionPlane is CollisionPlane.LeftWall or CollisionPlane.RightWall)
                    _rigidbody2D.linearVelocityY = 0;
            }

            if (!_wantJump || CurrentJumpCount >= maxJumpCount) return;

            // Jump from ground
            if (CurrentCollisionPlane == CollisionPlane.Ground)
            {
                _rigidbody2D.linearVelocityY = 0;
                _rigidbody2D.AddForceY(jumpForce);
                CurrentJumpCount = 1;
            }
            // Wall jump
            else if (CurrentCollisionPlane is CollisionPlane.LeftWall or CollisionPlane.RightWall)
            {
                _rigidbody2D.linearVelocity = Vector2.zero;
                _rigidbody2D.AddForceY(jumpForce);

                // Apply sideways force away from wall
                var wallJumpSign = CurrentCollisionPlane == CollisionPlane.LeftWall ? -1 : 1;
                var wallJumpForce = jumpForce * wallJumpSign * wallJumpForceMultiplier;
                _rigidbody2D.AddForceX(wallJumpForce);

                _timeSinceWallJump = 0;
                CurrentJumpCount = 1;
            }
            // Air jump (e.g double jump)
            else
            {
                _rigidbody2D.linearVelocityY = 0;
                _rigidbody2D.AddForceY(jumpForce);
                CurrentJumpCount++;
            }

            OnJump?.Invoke();
        }
        
        private CollisionPlane GetCollisionPlane()
        {
            if (_rigidbody2D.IsTouching(_groundFilter)) return CollisionPlane.Ground;
            if (_rigidbody2D.IsTouching(_leftWallFilter)) return CollisionPlane.LeftWall;
            if (_rigidbody2D.IsTouching(_rightWallFilter)) return CollisionPlane.RightWall;
            
            return CollisionPlane.None;
        }
    }
}