using Game;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button levelsButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            // TODO: Load tutorial
            playButton.onClick.AddListener(() => GameState.Instance.LoadLevel(GameState.Levels.Level1));

            levelsButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                MainMenu.Instance.levelsMenu.gameObject.SetActive(true);;
            });

            optionsButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                MainMenu.Instance.optionsMenu.gameObject.SetActive(true);
            });

            quitButton.onClick.AddListener(Application.Quit);
        }
    }
}