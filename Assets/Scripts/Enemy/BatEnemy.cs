using System;
using Game;
using UnityEngine;

namespace Enemy
{
    public class BatEnemy : MonoBehaviour
    {
        [SerializeField] private float damage = 25f;
        
        private MovePath _movePath;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;
        private bool _awake;
        
        private void Start()
        {
            _movePath = GetComponent<MovePath>();
            _movePath.enabled = false;
            
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player") || _awake) return;

            _awake = true;
            _animator.Play("BatStartFlying");
            _movePath.enabled = true;

            // TODO: Play sound once awoken
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            var player = other.gameObject;
            var playerHealth = player.GetComponent<Health>();
            playerHealth.TryDamage(damage);
        }

        private void Update()
        {
            if (!_awake) return;
            
            var moveX = Mathf.Sign(_rigidbody2D.linearVelocityX);
            _spriteRenderer.flipX = moveX > 0;
        }
    }
}
