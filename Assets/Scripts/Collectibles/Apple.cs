using System;
using Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace Collectibles
{
    public class Apple : MonoBehaviour
    {
        [SerializeField] private GameState gameState;
        [SerializeField] private ParticleSystem pickupParticleEffect;
        [SerializeField] private AudioClip pickupSound;
        /// <summary>
        /// Amount healed upon picking up the apple 
        /// </summary>
        [SerializeField] private float healAmount = 25f;
        
        private AudioSource _audioSource;
        private SpriteRenderer _spriteRenderer;
        private bool _active = true;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || !_active) return;

            gameState.score++;

            var playerHealth = other.GetComponent<Health>();
            playerHealth.Heal(healAmount);
            
            _audioSource.PlayOneShot(pickupSound);
            
            // Particle effect
            var particles = Instantiate(pickupParticleEffect, transform.position, Quaternion.identity);
            Destroy(particles.gameObject, 5f);
            
            // Remove apple after picking up
            _spriteRenderer.enabled = false;
            Destroy(gameObject, 3f);
            _active = false;
        }
    }
}
