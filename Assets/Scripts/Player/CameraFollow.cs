using Unity.Mathematics.Geometry;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new(0, 0, -10f);
    [SerializeField] private float speed = 1.0f;

    void Start()
    {
        transform.position = target.position + offset;
    }
    
    void Update()
    {
        //var alpha = Mathf.SmoothStep(0, 1, speed * Time.deltaTime);
        var alpha = 1 - Mathf.Exp(-speed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, target.position + offset, alpha);
    }
}