using GridHandler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using Audio;
using UnityEngine.Tilemaps;

public class Puzzle4Effects : MonoBehaviour
{

    PuzzleGridHandler puzzle;
    AudioManager audioManager;
    TileController tileController;

    bool effectsHappened = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the PuzzleGridHandler component
        puzzle = gameObject.GetComponent<PuzzleGridHandler>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        tileController = FindObjectOfType<Tilemap>().GetComponent<TileController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (puzzle.isSolved && !effectsHappened)
        {
            audioManager.TriggerSFX("river");

            // Play the "Playful" music
            audioManager.StartAmbiance("Forest");
            effectsHappened = true;
            StartCoroutine(tileController.ActivateRiver());
            GameObject.Find("UnderBridge").SetActive(false);
        }
    }
}
