using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameState : MonoBehaviour
    {
        private enum Levels
        {
            Tutorial,
            Level1,
            Level2,
            Level3,
            MainMenu,
        }
        
        [SerializeField] private TextMeshProUGUI scoreText;
        public int score;
        public static GameState Instance { get; private set; }

        private Levels _currentLevel;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
            
            // TEMP: Required on first load
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var ok = Enum.TryParse<Levels>(currentScene.name, out var currentLevel);
            Debug.Assert(ok);
            _currentLevel = currentLevel;
        }

        private void FixedUpdate()
        {
            scoreText.text = $"Score: {score}";
        }

        public void NextLevel()
        {
            score = 0;
            
            // TEMP: Should never happen since GameState is not present in MainMenu
            if (_currentLevel is Levels.MainMenu) return;

            // Load next scene
            // TODO: Level fade transition
            _currentLevel++;
            var nextLevelName = _currentLevel.ToString();
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
        }

        public void ReloadCurrentLevel()
        {
            score = 0;
            
            // TODO: Level fade transition
            var nextLevelName = _currentLevel.ToString();
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
        }
    }
}