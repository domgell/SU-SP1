using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new(0, 0, -10f);
    [SerializeField] private float speed = 1.0f;

    void Update()
    {
        var alpha = speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, alpha);
    }
}