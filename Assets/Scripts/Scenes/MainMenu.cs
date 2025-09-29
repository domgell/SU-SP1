using Menu;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes
{
    /// <summary>
    /// Global access to submenus in MainMenu scene
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        public static MainMenu Instance { get; private set; }
        public MainMenuController mainMenu;
        public LevelsMenuController levelsMenu;
        public OptionsMenuController optionsMenu;

        private void Awake()
        {
            Instance = this;
        }
    }
}