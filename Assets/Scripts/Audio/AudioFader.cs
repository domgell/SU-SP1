using System;
using System.Collections;
using UnityEngine;

namespace Audio
{
    public class AudioFader : MonoBehaviour
    {
        public IEnumerator FadeOut(AudioSource audioSource, float fadeTime, Action onComplete = null)
        {
            var startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
            onComplete?.Invoke();
        }
    }
}