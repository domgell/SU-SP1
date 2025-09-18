using System;
using UnityEngine;

public class WinScreenController : MonoBehaviour
{
    [SerializeField] private float timeSinceWin = 0;
    [SerializeField] private float timeUntilMenu = 1.75f;

    // Update is called once per frame
    void Update()
    {
        timeSinceWin += Time.deltaTime;
        if (timeSinceWin >= timeUntilMenu)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}
