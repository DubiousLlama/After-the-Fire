using GridHandler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using Audio;

public class Puzzle0Effects : MonoBehaviour
{
    PuzzleGridHandler puzzle;
    AudioManager audioManager;
    public GameObject plantInstructionsCanvas;//canvas for "move to the tile and press space to plant"

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
            Debug.Log("puzzle Solved");
            plantInstructionsCanvas.SetActive(false);
            effectsHappened = true;
        }
    }
}
