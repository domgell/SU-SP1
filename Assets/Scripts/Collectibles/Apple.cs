using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Collectibles
{
    public class Apple : MonoBehaviour
    {
        [SerializeField] private GameState gameState;
        [SerializeField] private ParticleSystem pickupParticleEffect;
        [SerializeField] private AudioClip pickupSound;
        
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

            var particles = Instantiate(pickupParticleEffect, transform.position, Quaternion.identity);
            Destroy(particles.gameObject, 5f);
            
            _audioSource.PlayOneShot(pickupSound);

            
            _spriteRenderer.enabled = false;
            Destroy(gameObject, 3f);
            _active = false;
        }
    }
}
