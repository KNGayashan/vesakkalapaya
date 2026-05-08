using UnityEngine;
using System.Collections;

public class MusicZoneTrigger : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;
    [SerializeField] private Transform playerTransform; // Drag your Player object here in the Inspector

    [Header("Settings")]
    public float fadeTime = 2.0f; 
    public float maxVolume = 1.0f;

    private Coroutine fadeCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the Sphere is the one we assigned as Player
        if (other.transform == playerTransform)
        {
            if (!audioSource.isPlaying) audioSource.Play();
            StartFade(maxVolume);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Fade out when the assigned player leaves the radius
        if (other.transform == playerTransform)
        {
            StartFade(0f);
        }
    }

    private void StartFade(float targetVolume)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeAudio(targetVolume));
    }

    private IEnumerator FadeAudio(float targetVolume)
    {
        float startVolume = audioSource.volume;
        float timer = 0;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / fadeTime);
            yield return null;
        }

        audioSource.volume = targetVolume;
        
        // Stop audio fully if faded out to save resources
        if (audioSource.volume <= 0)
        {
            audioSource.Stop();
        }
    }
}