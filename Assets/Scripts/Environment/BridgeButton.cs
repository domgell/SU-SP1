using System;
using UnityEngine;

namespace Environment
{
    public class BridgeButton : MonoBehaviour
    {
        [SerializeField] private Bridge bridge;
        [SerializeField] private Color pressedColor = new(0.5f, 0.5f, 0.5f, 1f);
        
        private SpriteRenderer _spriteRenderer;
        private AudioSource _audioSource;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            _spriteRenderer.color = pressedColor;
            
            bridge.Activate();
            
            // TODO: Play press sound
        }
    }
}
