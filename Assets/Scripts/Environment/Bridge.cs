using UnityEngine;
using UnityEngine.Tilemaps;

public class Bridge : MonoBehaviour
{
    private TilemapRenderer _tilemapRenderer;
    private Rigidbody2D _rigidbody2D;

    [SerializeField] private Color inactiveColor = new(0.5f, 0.5f, 0.5f, 0.65f);
    
    void Start()
    {
        _tilemapRenderer = GetComponent<TilemapRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        
        _tilemapRenderer.material.color = inactiveColor;
        _rigidbody2D.simulated = false;
    }

    public void Activate()
    {
        _tilemapRenderer.material.color = Color.white;
        _rigidbody2D.simulated = true;
    }
}
