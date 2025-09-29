using System;
using Environment;
using UnityEngine;

namespace Collectibles
{
    public class Key : MonoBehaviour
    {
        [SerializeField] private Door targetDoor;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            // TODO: Play pickup sound
            targetDoor.Locked = false;

            Destroy(gameObject);
        }
    }
}