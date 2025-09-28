using Player;
using UnityEngine;

namespace Traps
{
    public class PurpleGrass : MonoBehaviour
    {
        private float _defaultMovementSpeed;
        private float _defaultJumpForce;
        private int _defaultMaxJumpCount;

        [SerializeField] private float newPlayerJumpForce = 200f;
        [SerializeField] private float newPlayerMovementSpeed = 5f;
    
        // TODO: Purple particles when jumping, special sound when jumping
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            var player = other.gameObject;
            var playerMovement = player.GetComponent<PlayerMovement>();
        
            playerMovement.canWallJump = false;
            _defaultMovementSpeed = playerMovement.movementSpeed;
            _defaultJumpForce = playerMovement.jumpForce;
            _defaultMaxJumpCount = playerMovement.maxJumpCount;
        
            playerMovement.movementSpeed = newPlayerMovementSpeed;
            playerMovement.jumpForce = newPlayerJumpForce;
            playerMovement.maxJumpCount = 1;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
        
            var player = other.gameObject;
            var playerMovement = player.GetComponent<PlayerMovement>();
        
            playerMovement.canWallJump = true;
            playerMovement.movementSpeed = _defaultMovementSpeed;
            playerMovement.jumpForce = _defaultJumpForce;
            playerMovement.maxJumpCount = _defaultMaxJumpCount;
        }
    }
}
