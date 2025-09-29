using System;
using System.Linq;
using Audio;
using Scenes;
using TMPro;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Global access to Score and Loading levels
    /// </summary>
    public class GameState : MonoBehaviour
    {
        public enum Levels
        {
            Level1,
            Level2,
            Level3,
            MainMenu,
        }

        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private AudioSource levelMusic;
        
        public int score;
        public static GameState Instance { get; private set; }

        private AudioSource _audioSource;
        private AudioFader _audioFader;
        private ScreenFader _screenFader;

        private Levels _currentLevel;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioFader = GetComponent<AudioFader>();
            _screenFader = GetComponent<ScreenFader>();
        }

        private void Awake()
        {
            Instance = this;

            // TEMP
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var ok = Enum.TryParse<Levels>(currentScene.name, out var currentLevel);
            Debug.Assert(ok);
            _currentLevel = currentLevel;
        }

        private void Update()
        {
            if (_currentLevel == Levels.MainMenu) return;

            // TEMP: No score text set in MainMenu
            if (scoreText) scoreText.text = $"Score: {score}";

            if (Input.GetKeyDown(KeyCode.Escape)) LoadLevel(Levels.MainMenu);
        }

        public void LoadNextLevel()
        {
            score = 0;

            // TEMP: Should never happen since GameState is not present in MainMenu
            if (_currentLevel is Levels.MainMenu) return;

            // Load next scene
            _currentLevel++;
            LoadLevel(_currentLevel);
        }

        public void LoadLevel(Levels level)
        {
            score = 0;
            _currentLevel = level;
            var nextLevelName = _currentLevel.ToString();

            StartCoroutine(_audioFader.FadeOut(levelMusic, 0.99f));
            
            // Fade out, level transition, fade back in
            StartCoroutine(_screenFader.FadeOut(onComplete: () =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
                StartCoroutine(_screenFader.FadeIn());
            }));
        }

        public void ReloadCurrentLevel() => LoadLevel(_currentLevel);
    }
}