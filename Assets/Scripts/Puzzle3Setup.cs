using GridHandler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Puzzle3Setup : MonoBehaviour
{
    public GameObject infertileSoil;
    public GameObject richSoil;

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
        puzzleGridHandler.setTile(3, 1, infertileSoil);
        puzzleGridHandler.setTile(0, 2, infertileSoil);
        puzzleGridHandler.setTile(0, 1, infertileSoil);
        puzzleGridHandler.setTile(1, 3, infertileSoil);
        puzzleGridHandler.setTile(0, 0, infertileSoil);
        puzzleGridHandler.setTile(0, 1, infertileSoil);
        puzzleGridHandler.setTile(0, 2, infertileSoil);
        puzzleGridHandler.setTile(1, 2, infertileSoil);
        puzzleGridHandler.setTile(1, 0, infertileSoil);
        puzzleGridHandler.setTile(2, 0, infertileSoil);
        puzzleGridHandler.setTile(2, 2, richSoil);


    }
}
