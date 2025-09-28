using System;
using Game;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Vector2 respawnPoint;
        [SerializeField] private Slider healthBar;
        [Space]
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip landSound;
        [SerializeField] private AudioClip moveSound;
        [SerializeField] private AudioClip damageSound;
        [SerializeField] private AudioClip deathSound;
        
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Health _health;
        private PlayerMovement _playerMovement;
        private AudioSource _audioSource;
        
        private readonly Vector4 _hitFlashColor = new(1, 0.1f, 0.1f, 1);
        
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _health = GetComponent<Health>();
            _playerMovement = GetComponent<PlayerMovement>();
            _audioSource = GetComponent<AudioSource>();

            _health.OnDeath += OnDeath;
            _health.OnDamage += OnDamage;
            _playerMovement.OnJump += () => _audioSource.PlayOneShot(jumpSound);
            _playerMovement.OnLand += () => _audioSource.PlayOneShot(landSound);
        }

        private void OnDamage(float amount)
        {
            _audioSource.PlayOneShot(damageSound);
        }

        private void OnDeath()
        {
            _audioSource.PlayOneShot(deathSound);
            
            // Respawn
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
            _animator.SetBool("isGrounded", _playerMovement.currentCollisionPlane is PlayerMovement.CollisionPlane.Ground);
            _animator.SetBool("isOnWall", _playerMovement.currentCollisionPlane is PlayerMovement.CollisionPlane.LeftWall or PlayerMovement.CollisionPlane.RightWall);
            _animator.SetInteger("jumpCount", _playerMovement.currentJumpCount);
            _animator.SetBool("isFalling", _rigidbody2D.linearVelocityY < 0);

            healthBar.value = _health.CurrentHealth / _health.MaxHealth;
        }

        private void OnDrawGizmosSelected()
        {
            Debug.DrawLine(respawnPoint, respawnPoint + Vector2.up, Color.red);
        }
    }
}