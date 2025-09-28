using UnityEngine;

public class MenuController : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Button quitButton;


    private void Start()
    {
        quitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}