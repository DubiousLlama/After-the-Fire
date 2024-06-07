using GridHandler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using Audio;

public class Puzzle3Effects : MonoBehaviour
{

    public GameObject bigLog;

    PuzzleGridHandler puzzle;
    AudioManager audioManager;

    bool effectsHappened = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the PuzzleGridHandler component
        puzzle = gameObject.GetComponent<PuzzleGridHandler>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzle.isSolved && !effectsHappened)
        {
            // Play the "Playful" music
            // audioManager.StartAmbiance("Forest");

            effectsHappened = true;

            StartCoroutine(FadeOut(bigLog.GetComponent<SpriteRenderer>()));
            Invoke("DisableObject", 1.0f);
        }


    }

    // Fade out the sprite renderer of the object over time
    IEnumerator FadeOut(SpriteRenderer sprite)
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            Color c = sprite.color;
            c.a = f;
            sprite.color = c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Disable the object
    void DisableObject()
    {
        bigLog.SetActive(false);
    }
}
