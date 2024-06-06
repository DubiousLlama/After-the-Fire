using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;

[ExecuteInEditMode]
public class Puzzle2Setup : MonoBehaviour
{
    public GameObject infertileSoil;

    // Start is called before the first frame update
    void Start()
    {
        // Get the puzzle grid handler script attached to this game object
        PuzzleGridHandler puzzleGridHandler = GetComponent<PuzzleGridHandler>();
        if (puzzleGridHandler == null)
        {
            Debug.LogAssertion("handler is null");
        }

        // Set two of the cells to be blocked with a boulder
        //puzzleGridHandler.setTile(2, 0, infertileSoil);
        puzzleGridHandler.setTile(3, 0, infertileSoil);

    }
}
