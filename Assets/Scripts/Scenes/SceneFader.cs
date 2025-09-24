using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image image;
    public float fadeDuration = 1f;

    private void Start()
    {
        image.raycastTarget = false; 
        StartCoroutine(FadeOut());
    }

    public void PlayButtonClicked(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    public void QuitButtonClicked()
    {
        StartCoroutine(FadeAndQuit());
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        image.raycastTarget = true; 

        float t = 0;
        Color c = image.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = t / fadeDuration;
            image.color = c;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeAndQuit()
    {
        image.raycastTarget = true; 

        float t = 0;
        Color c = image.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = t / fadeDuration;
            image.color = c;
            yield return null;
        }

        Application.Quit();

        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator FadeOut()
    {
        float t = 0;
        Color c = image.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / fadeDuration);
            image.color = c;
            yield return null;
        }

        c.a = 0f;
        image.color = c;

        image.raycastTarget = false; 
    }
}
