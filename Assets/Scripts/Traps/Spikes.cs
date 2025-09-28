using Game;
using UnityEngine;

namespace Traps
{
    public class Spikes : MonoBehaviour
    {
        [SerializeField] private float damage = 50f;
    
        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            var playerHealth = other.GetComponent<Health>();
            playerHealth.TryDamage(damage);
        }
    }
}
