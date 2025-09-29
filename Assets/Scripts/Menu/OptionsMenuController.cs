using Scenes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace Menu
{
    public class OptionsMenuController : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Button backButton;
        
        private void Start()
        {
            volumeSlider.onValueChanged.AddListener(value => AudioListener.volume = value);
            volumeSlider.value = AudioListener.volume;
            
            backButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                MainMenu.Instance.mainMenu.gameObject.SetActive(true);
            });
        }
    }
}