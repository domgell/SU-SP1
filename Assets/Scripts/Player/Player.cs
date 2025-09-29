using System;
using Game;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        [Header("Sound")] [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip damageSound;

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

            _health.OnDeath += () => GameState.Instance.ReloadCurrentLevel();
            _health.OnDamage += _ => _audioSource.PlayOneShot(damageSound);
            _playerMovement.OnJump += () => _audioSource.PlayOneShot(jumpSound);
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
            _animator.SetBool("isOnWall", _playerMovement.currentCollisionPlane.IsWall());
            _animator.SetInteger("jumpCount", _playerMovement.currentJumpCount);
            _animator.SetBool("isFalling", _rigidbody2D.linearVelocityY < 0);

            healthBar.value = _health.CurrentHealth / _health.MaxHealth;
        }
    }
}