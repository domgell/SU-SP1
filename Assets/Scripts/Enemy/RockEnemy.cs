using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class RockEnemy : MonoBehaviour
    {
        [SerializeField] private float bounceForce = 300;
        [SerializeField] private float damage = 25;
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

            _spriteRenderer.enabled = false;
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

            // Enemy hit
            if (playerAngle > damageAngle)
            {
                // Player bounce off enemy
                var playerRigidBody = player.GetComponent<Rigidbody2D>();
                playerRigidBody.linearVelocityY = 0;
                playerRigidBody.AddForceY(bounceForce);

                _health.TryDamage(1f);
            }
            // Player hit
            else
            {
                var playerHealth = player.GetComponent<Health>();
                playerHealth.TryDamage(damage);
            }
        }
    }
}