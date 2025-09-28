using System;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallingForce = 400f;
    private bool _falling;
    
    private Rigidbody2D _rigidbody2D;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        _falling = true;
        Destroy(gameObject, 15f);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        other.rigidbody.AddForceY(_rigidbody2D.linearVelocityY);
    }

    private void FixedUpdate()
    {
        if (!_falling) return;
        
        _rigidbody2D.AddForceY(-fallingForce);
    }
}
