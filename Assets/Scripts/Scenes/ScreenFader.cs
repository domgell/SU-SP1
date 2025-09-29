using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes
{
    public class ScreenFader : MonoBehaviour
    {
        public Image image;
        public float fadeDuration = 1f;
        
        public IEnumerator FadeOut(Action onComplete = null)
        {
            var t = 0f;
            var color = image.color;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
                image.color = color;
                yield return null;
            }
            
            onComplete?.Invoke();
        }

        public IEnumerator FadeIn(Action onComplete = null)
        {
            var t = 0f;
            var currentColor = image.color;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                currentColor.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
                image.color = currentColor;
                yield return null;
            }
            
            onComplete?.Invoke();
        }
    }
}