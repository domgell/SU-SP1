using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(() => { UnityEngine.SceneManagement.SceneManager.LoadScene("Level1"); });
        quitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}