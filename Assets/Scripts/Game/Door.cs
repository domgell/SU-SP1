using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked;
    private Collider2D doorCollider;
    private SpriteRenderer spriteRenderer;

    [Header("Door Sprites")]
    public Sprite lockedSprite;
    public Sprite unlockedSprite;

    void Start()
    {
        locked = true;
        doorCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateDoorState();
    }

    void Update()
    {
        UpdateDoorState();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            locked = false;
            UpdateDoorState();


            Destroy(other.gameObject);
        }
    }

    private void UpdateDoorState()
    {
        if (doorCollider != null)
            doorCollider.enabled = locked;

        if (spriteRenderer != null)
        {
            if (locked && lockedSprite != null)
                spriteRenderer.sprite = lockedSprite;
            else if (!locked && unlockedSprite != null)
                spriteRenderer.sprite = unlockedSprite;
        }
    }
}
