using UnityEngine;

namespace Game
{
    public class Platform : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
        
            other.rigidbody.AddForce(_rigidbody2D.linearVelocity);
        }
    }
}