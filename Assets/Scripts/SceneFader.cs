using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Audio;

// Code credit ChatGPT. Prompt: In Unity, I want to fade to black in between scenes. How can I do this?
public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    private float bw = 0;
    AudioManager audioManager;

    private void Start()
    {
        // Get the name of the scene that is currently loaded
        string sceneName = SceneManager.GetActiveScene().name;
        audioManager = AudioManager.instance;

        if (sceneName == "Level One")
        {
            bw = 1;
            fadeImage.color = new Color(bw, bw, bw, 0);

            StartCoroutine(FadeIn());
        }
        else if (sceneName == "Tutorial")
        {
            bw = 0;
            fadeImage.color = new Color(bw, bw, bw, 0);
            StartCoroutine(FadeIn());
        }

    }

    public void FadeToScene(string sceneName)
    {
        Debug.Log("Fade Triggered");
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(bw, bw, bw, 1 - (elapsedTime / fadeDuration));
            yield return null;
        }
        fadeImage.color = new Color(bw, bw, bw, 0);
    }

    private IEnumerator FadeOut(string sceneName)
    {
        Debug.Log("Fading out to " + sceneName);
        if (sceneName == "Level One")
        {
            bw = 1;
        }
        else
        {
            bw = 0;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            audioManager.musicSource.volume = 1f - (elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(bw, bw, bw, elapsedTime / fadeDuration);
            yield return null;
        }
        fadeImage.color = new Color(bw, bw, bw, 1);
        audioManager.musicSource.volume = 1;
        SceneManager.LoadScene(sceneName);
    }
}
