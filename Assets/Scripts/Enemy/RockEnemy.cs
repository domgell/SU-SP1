using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class RockEnemy : MonoBehaviour
    {
        [SerializeField] private Transform pathBegin;
        [SerializeField] private Transform pathEnd;
        [SerializeField] private float movementSpeed = 100;
        [SerializeField] private float bounceForce = 300;

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        private bool _movingToEnd = true;
        private bool _alive = true;
        private const float targetMinDistance = 0.1f;

        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (!_alive) return;

            // Distance and direction to target path
            var targetPath = _movingToEnd ? pathEnd : pathBegin;
            Vector2 toTarget = targetPath.position - transform.position;
            var targetDistance = toTarget.magnitude;

            // Set target path for next frame 
            if (targetDistance < targetMinDistance)
            {
                _movingToEnd = !_movingToEnd;
            }

            // Apply movement
            var moveX = Mathf.Sign(toTarget.x);
            _rigidbody2D.linearVelocityX = moveX * movementSpeed * Time.deltaTime;

            // Flip sprite towards movement direction
            _spriteRenderer.flipX = moveX > 0;

            Debug.DrawLine(targetPath.position, targetPath.position + Vector3.up);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject;
            if (!player.CompareTag("Player")) return;

            Vector2 toPlayer = transform.position - player.transform.position;
            var playerAngle = Vector2.Angle(toPlayer.normalized, Vector2.up);

            // Enemy hit
            if (playerAngle > 90)
            {
                var playerRigidBody = player.GetComponent<Rigidbody2D>();
                playerRigidBody.linearVelocityY = 0;
                playerRigidBody.AddForceY(bounceForce);
                
                // TODO: Enemy dead
            }
            // Player hit
            else
            {
                // TODO: Player take damage
            }
        }
    }
}