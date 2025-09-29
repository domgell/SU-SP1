using UnityEngine;

namespace Scenes
{
    public class AnimatedMenuBackground : MonoBehaviour
    {
        [SerializeField] private float movementAmplitude = 0.6f;
        [SerializeField] private float movementFrequency = 0.9f;
    
        private void Update()
        {
            var x = movementAmplitude * Mathf.Sin(Time.time * movementFrequency);
            var y = movementAmplitude * Mathf.Cos(Time.time * movementFrequency);
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
