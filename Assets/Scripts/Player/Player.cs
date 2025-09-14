using System;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Vector2 respawnPoint;
        
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Health _health;
        private PlayerMovement _playerMovement;
        private readonly Vector4 _hitFlashColor = new(1, 0.1f, 0.1f, 1);

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _health = GetComponent<Health>();
            _playerMovement = GetComponent<PlayerMovement>();

            _health.OnDeath += Respawn;
        }

        private void Respawn()
        {
            _health.Reset();
            transform.position = respawnPoint;
            _rigidbody2D.linearVelocity = Vector2.zero;
        }

        private void Update()
        {
            // Flash red on damage
            var alpha = Mathf.Clamp01(_health.TimeSinceDamage / _health.DamageCooldown);
            _spriteRenderer.color = Color.Lerp(_hitFlashColor, Color.white, alpha);

            // Flip sprite in movement direction
            var moveX = _playerMovement.MovementInputX;
            if (moveX != 0) _spriteRenderer.flipX = moveX < 0;

            // Update animation
            _animator.SetFloat("movementSpeedX", Mathf.Abs(moveX));
            _animator.SetBool("isGrounded", _playerMovement.IsGrounded);
            _animator.SetInteger("jumpCount", _playerMovement.CurrentJumpCount);
            _animator.SetBool("isFalling", _rigidbody2D.linearVelocityY < 0);
        }

        private void OnDrawGizmosSelected()
        {
            Debug.DrawLine(respawnPoint, respawnPoint + Vector2.up, Color.red);
        }
    }
}