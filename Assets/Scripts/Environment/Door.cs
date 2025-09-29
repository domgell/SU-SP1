using Game;
using UnityEngine;

namespace Environment
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private bool lockedByDefault;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite unlockedSprite;

        private bool _locked = true;

        public bool Locked
        {
            get => _locked;
            set
            {
                _locked = value;
                _spriteRenderer.sprite = value ? lockedSprite : unlockedSprite;
            }
        }

        private SpriteRenderer _spriteRenderer;

        public void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Locked = lockedByDefault;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player") || Locked) return;
        
            // TODO: Play unlock sound
        
            GameState.Instance.LoadNextLevel();
        }
    }
}