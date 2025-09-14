using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class RockEnemy : MonoBehaviour
    {
        [SerializeField] private Vector2 pathBegin;
        [SerializeField] private Vector2 pathEnd;
        [SerializeField] private float movementSpeed = 100;
        [SerializeField] private float bounceForce = 300;
        [SerializeField] private float damage = 25;
        [SerializeField] private float damageAngle = 100f;

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Health _health;
        private bool _movingToEnd = true;
        private const float targetMinDistance = 0.1f;

        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _health = GetComponent<Health>();

            _health.OnDamage += amount => { Debug.Log("Enemy damaged"); };

            _health.OnDeath += () => { Destroy(gameObject.gameObject); };
        }

        private void FixedUpdate()
        {
            // Distance and direction to target path
            var targetPath = _movingToEnd ? pathEnd : pathBegin;
            var toTarget = targetPath - new Vector2(transform.position.x, transform.position.y);
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
        }

        private void OnDrawGizmosSelected()
        {
            Debug.DrawLine(pathBegin, pathBegin + Vector2.up, Color.red);
            Debug.DrawLine(pathEnd, pathEnd + Vector2.up, Color.red);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject;
            if (!player.CompareTag("Player")) return;

            Vector2 toPlayer = transform.position - player.transform.position;
            var playerAngle = Vector2.Angle(toPlayer.normalized, Vector2.up);

            // Enemy hit
            if (playerAngle > damageAngle)
            {
                // Player bounce off enemy
                var playerRigidBody = player.GetComponent<Rigidbody2D>();
                playerRigidBody.linearVelocityY = 0;
                playerRigidBody.AddForceY(bounceForce);

                _health.TryDamage(1f);
                Debug.Log("Enemy hit!");
            }
            // Player hit
            else
            {
                var playerHealth = player.GetComponent<Health>();
                playerHealth.TryDamage(damage);
                Debug.Log("Player hit!");
            }
        }
    }
}