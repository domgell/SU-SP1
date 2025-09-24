using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [SerializeField] private Button quitButton;


    private void Start()
    {

        quitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}