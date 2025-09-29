using UnityEngine;

namespace Game
{
    public class MovePath : MonoBehaviour
    {
        [SerializeField] private Vector2 pathBegin;
        [SerializeField] private Vector2 pathEnd;
        [SerializeField] private float movementSpeed = 5f;
    
        public bool movingToEnd = false;
        private const float targetMinDistance = 0.01f;
        private Rigidbody2D _rigidbody2D;
    
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnDrawGizmosSelected()
        {
            Debug.DrawLine(pathBegin, pathBegin + Vector2.up, movingToEnd ? Color.green : Color.red);
            Debug.DrawLine(pathEnd, pathEnd + Vector2.up, movingToEnd ? Color.red : Color.green);
        }

        private void FixedUpdate()
        {
            // Select current target
            var targetPosition = movingToEnd ? pathEnd : pathBegin;
            var currentPosition = (Vector2)transform.position;
            var toTarget = targetPosition - currentPosition;
            var targetDistance = toTarget.magnitude;

            // If within tolerance, swap target and exit early
            if (targetDistance < targetMinDistance)
            {
                movingToEnd = !movingToEnd;
                _rigidbody2D.linearVelocity = Vector2.zero;
                return;
            }

            // Movement step this frame
            var step = movementSpeed * Time.deltaTime;

            // If step is larger than distance, clamp to stop at target
            if (step >= targetDistance)
            {
                _rigidbody2D.MovePosition(targetPosition);
                _rigidbody2D.linearVelocity = Vector2.zero;
            }
            else
            {
                var toTargetDirection = toTarget / targetDistance;
                _rigidbody2D.linearVelocity = toTargetDirection * movementSpeed;
            }
        }
    }
}
