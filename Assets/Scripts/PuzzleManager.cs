using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;
using UnityEditor;
using UnityEngine.Tilemaps;

public class PuzzleManager : MonoBehaviour
{
    public GameObject[] puzzleList;

    // Create a dictionary to store the puzzles locaton and the puzzle object
    public Dictionary<Vector3, GameObject> puzzleDict = new Dictionary<Vector3, GameObject>();

    GameObject player;

    ReferenceGUI active;

    void Start()
    {

        foreach (GameObject puzzle in puzzleList)
        {
            // Get the puzzle position
            Vector3 puzzlePos = puzzle.transform.position;

            // Add the puzzle to the dictionary
            puzzleDict.Add(puzzlePos, puzzle);

            Debug.Log(puzzle);
            Debug.Log(puzzlePos);
        }

        // Get the player object
        player = GameObject.Find("Player");
    }

    public void Update()
    {
        // Get the distance between the player and the closest puzszle
        Vector3 closestPuzzle = getClosest();

        // Get the distance between the player and the closest puzzle
        float distance = Vector3.Distance(player.transform.position, closestPuzzle);
        // Debug.Log(distance);

        PuzzleGridHandler c = puzzleDict[closestPuzzle].GetComponent<PuzzleGridHandler>();
        ReferenceGUI g = puzzleDict[closestPuzzle].GetComponent<ReferenceGUI>();

        if (g != null)
        {
            //If the player is close enough to the puzzle, display the requirements
            if (distance < 1f + ((c.width + c.height) / 2f * 0.639204f))
            {
                if (g.visible == false)
                {
                    g.toggleGUI();
                    active = g;
                }
            }
            else
            {
                if (g.visible == true)
                {
                    g.toggleGUI();
                }
            }
        }

    }

    public bool Place(string plant)
    {

        Vector3 closestPuzzle = getClosest();

        PuzzleGridHandler c = puzzleDict[closestPuzzle].GetComponent<PuzzleGridHandler>();

        if (closestPuzzle != Vector3.zero)
        {
            return c.Place(plant);
        }
        else
        {
            Debug.Log("No puzzle found");
            return false;
        }
    }

    public string getContent() {
        PuzzleGridHandler c = puzzleDict[getClosest()].GetComponent<PuzzleGridHandler>();

        Vector2 tile = c.PositionToTile(player.transform.position);

        return c.GetContent((int)tile.x, (int)tile.y);
    }

    public bool checkSolved() {
        PuzzleGridHandler c = puzzleDict[getClosest()].GetComponent<PuzzleGridHandler>();

        return c.isSolved;
    }

    private Vector3 getClosest()
    {
        // Call the Place function of the puzzle the player is closest to
        Vector3 playerPos = player.transform.position;

        float minDistance = Mathf.Infinity;
        Vector3 closestPuzzle = Vector3.zero;

        foreach (Vector3 location in puzzleDict.Keys)
        {
            // Get the distance between the player and the puzzle
            float distance = Vector3.Distance(playerPos, location);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPuzzle = location;
            }
        }

        return closestPuzzle;
    }



}
