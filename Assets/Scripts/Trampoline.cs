using System;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float bounceForce = 500f;
    [SerializeField] private AudioClip bounceSound;
    
    private AudioSource _audioSource;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var playerRigidBody = other.GetComponent<Rigidbody2D>();
        
        playerRigidBody.linearVelocityY = 0;
        playerRigidBody.AddForceY(bounceForce);
        _audioSource.PlayOneShot(bounceSound);
    }
}
