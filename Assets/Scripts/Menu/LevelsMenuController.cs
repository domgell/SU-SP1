using Game;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class LevelsMenuController : MonoBehaviour
    {
        [SerializeField] private Button level1Button;
        [SerializeField] private Button leve2Button;
        [SerializeField] private Button level3Button;
        [SerializeField] private Button backButton;

        private void Start()
        {
            level1Button.onClick.AddListener(() => GameState.Instance.LoadLevel(GameState.Levels.Level1));
            leve2Button.onClick.AddListener(() => GameState.Instance.LoadLevel(GameState.Levels.Level2));
            level3Button.onClick.AddListener(() => GameState.Instance.LoadLevel(GameState.Levels.Level3));

            backButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                MainMenu.Instance.mainMenu.gameObject.SetActive(true);
            });
        }
    }
}