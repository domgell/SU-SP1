using System;
using Environment;
using UnityEngine;

namespace Collectibles
{
    public class Key : MonoBehaviour
    {
        [SerializeField] private Door targetDoor;
        [SerializeField] private AudioClip pickupSound;
        
        private AudioSource _audioSource;
        private SpriteRenderer _spriteRenderer;
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            targetDoor.Locked = false;
            _audioSource.PlayOneShot(pickupSound);

            _spriteRenderer.enabled = false;
            Destroy(gameObject, 2f);
        }
    }
}