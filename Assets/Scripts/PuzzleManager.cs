using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;

public class PuzzleManager : MonoBehaviour
{
    public GameObject[] puzzleList;

    // Create a dictionary to store the puzzles locaton and the puzzle object
    public Dictionary<Vector3, GameObject> puzzleDict = new Dictionary<Vector3, GameObject>();

    GameObject player;

    void Start()
    {
        foreach (GameObject puzzle in puzzleList)
        {
            // Get the puzzle position
            Vector3 puzzlePos = puzzle.transform.position;

            // Add the puzzle to the dictionary
            puzzleDict.Add(puzzlePos, puzzle);
        }

        // Get the player object
        player = GameObject.Find("Player");
    }

    public void Place(string plant)
    {
        Vector3 closestPuzzle = getClosest();

        if (closestPuzzle != Vector3.zero)
        {
            puzzleDict[closestPuzzle].GetComponent<PuzzleGridHandler>().Place(plant);
        }
        else
        {
            Debug.Log("No puzzle found");
        }
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
