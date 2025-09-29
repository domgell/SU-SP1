using System;
using Game;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class RockEnemy : MonoBehaviour
    {
        /// <summary>
        /// Bounce force applied to the player when jumping on the enemy
        /// </summary>
        [SerializeField] private float bounceForce = 300;
        /// <summary>
        /// Damage applied to the player when successfully hit
        /// </summary>
        [SerializeField] private float damage = 25;
        /// <summary>
        /// Knockback force applied to the player when successfully hit
        /// </summary>
        [SerializeField] private float damageKnockbackForce = 500;
        /// <summary>
        /// Angle in degrees which determines whether player or enemy was hit.
        ///
        /// For example: If player lands on top of enemy, the enemy should be hit.
        /// If the player is to the side of the enemy, the player should be hit.
        /// </summary>
        [SerializeField] private float damageAngle = 100f;
        [SerializeField] private AudioClip deathSound;
        [SerializeField] private ParticleSystem deathParticleSystem;

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Health _health;
        private AudioSource _audioSource;
        private bool _active = true;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _health = GetComponent<Health>();
            _audioSource = GetComponent<AudioSource>();
            
            _health.OnDeath += Death;
        }

        private void Death()
        {
            _audioSource.PlayOneShot(deathSound);

            var particles = Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
            Destroy(particles.gameObject, 5f);

            // TEMP
            _spriteRenderer.enabled = false;
            _rigidbody2D.simulated = false;
            Destroy(gameObject, 3f);
            _active = false;
        }

        private void FixedUpdate()
        {
            if (!_active) return;

            // Flip sprite in movement direction
            var moveX = Mathf.Sign(_rigidbody2D.linearVelocityX);
            _spriteRenderer.flipX = moveX > 0;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_active) return;

            var player = collision.gameObject;
            if (!player.CompareTag("Player")) return;

            Vector2 toPlayer = transform.position - player.transform.position;
            var playerAngle = Vector2.Angle(toPlayer.normalized, Vector2.up);
            
            // Player or self hit
            if (playerAngle > damageAngle) OnSelfHit(player);
            else OnPlayerHit(player, toPlayer);
        }

        private void OnPlayerHit(GameObject player, Vector2 toPlayer)
        {
            // Try to damage player
            var playerHealth = player.GetComponent<Health>();
            var hit = playerHealth.TryDamage(damage);
            if (!hit) return;

            // Apply knockback on successful hit
            var playerRigidBody = player.GetComponent<Rigidbody2D>();
            var knockbackDir = toPlayer + Vector2.down;
            var knockbackForce = knockbackDir * -damageKnockbackForce;
            playerRigidBody.AddForce(knockbackForce);
        }

        private void OnSelfHit(GameObject player)
        {
            // Player bounce off enemy
            var playerRigidBody = player.GetComponent<Rigidbody2D>();
            playerRigidBody.linearVelocityY = 0;
            playerRigidBody.AddForceY(bounceForce);

            _health.TryDamage(1f);
        }
    }
}