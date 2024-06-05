using GridHandler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using Audio;

public class Puzzle0Effects : MonoBehaviour
{

    public GameObject fountainSound;
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

        }
    }
}
