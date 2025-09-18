using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        other.rigidbody.linearVelocity += _rigidbody2D.linearVelocity * Time.deltaTime;
    }
}
