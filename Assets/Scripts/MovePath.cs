using System;
using UnityEngine;

public class MovePath : MonoBehaviour
{
    [SerializeField] private Vector2 pathBegin;
    [SerializeField] private Vector2 pathEnd;
    [SerializeField] private float movementSpeed = 75f;
    
    private bool _movingToEnd = true;
    private const float targetMinDistance = 0.01f;
    private Rigidbody2D _rigidbody2D;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(pathBegin, pathBegin + Vector2.up, _movingToEnd ? Color.green : Color.red);
        Debug.DrawLine(pathEnd, pathEnd + Vector2.up, _movingToEnd ? Color.red : Color.green);
    }

    private void FixedUpdate()
    {
        // Distance and direction to target path
        var targetPosition = _movingToEnd ? pathEnd : pathBegin;
        var currentPosition = new Vector2(transform.position.x, transform.position.y);
        var toTarget = targetPosition - currentPosition;
        var targetDistance = toTarget.magnitude;

        // Set target path for next update 
        if (targetDistance < targetMinDistance)
        {
            _movingToEnd = !_movingToEnd;
        }

        // Apply movement towards target
        var toTargetDirection = toTarget / targetDistance;
        var movement = toTargetDirection * (movementSpeed * Time.deltaTime);
        _rigidbody2D.linearVelocity = movement;
    }
}
