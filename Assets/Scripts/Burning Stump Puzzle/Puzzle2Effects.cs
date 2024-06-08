using GridHandler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using Audio;

public class Puzzle2Effects : MonoBehaviour
{

    //public GameObject fireStump;
    //public GameObject regStump;
    public GameObject fireSound;

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
            audioManager.StartMusicFade("Peaceful");
            audioManager.TriggerSFX("Extinguish");
            effectsHappened = true;

            //fireStump.SetActive(false);
            //regStump.SetActive(true);
            fireSound.SetActive(false);

        }
    }
}
