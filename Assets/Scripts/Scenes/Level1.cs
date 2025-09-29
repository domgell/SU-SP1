using Game;
using UnityEngine;

namespace Scenes
{
    public class Level1 : MonoBehaviour
    {
        [SerializeField] private int applesToCollect = 10;

        private void FixedUpdate()
        {
            if (GameState.Instance.score >= applesToCollect)
            {
                GameState.Instance.LoadNextLevel();
            }
        }
    }
}