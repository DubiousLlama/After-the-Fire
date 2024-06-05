using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// Code credit ChatGPT. Prompt: In Unity, I want to fade to black in between scenes. How can I do this?
public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    private float bw = 0;

    private void Start()
    {
        // Get the name of the scene that is currently loaded
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Level One")
        {
            // Ensure the Image is initially transparent
            bw = 1;
            fadeImage.color = new Color(bw, bw, bw, 0);

            StartCoroutine(FadeIn());
        }
        else if (sceneName == "Tutorial")
        {
            // Ensure the Image is initially transparent
            bw = 0;
            fadeImage.color = new Color(bw, bw, bw, 0);
            StartCoroutine(FadeIn());
        }

    }

    public void FadeToScene(string sceneName)
    {
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
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(bw, bw, bw, elapsedTime / fadeDuration);
            yield return null;
        }
        fadeImage.color = new Color(bw, bw, bw, 1);
        SceneManager.LoadScene(sceneName);
    }
}
