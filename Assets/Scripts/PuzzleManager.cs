using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;
using UnityEditor;

public class PuzzleManager : MonoBehaviour
{
    public GameObject[] puzzleList;

    // Create a dictionary to store the puzzles locaton and the puzzle object
    public Dictionary<Vector3, GameObject> puzzleDict = new Dictionary<Vector3, GameObject>();

    GameObject player;

    GameObject reqCanvas;

    void Start()
    {
        reqCanvas = GameObject.Find("ReqCanvas");

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
        // Get the distance between the player and the closest puzzle
        Vector3 closestPuzzle = getClosest();

        // Get the distance between the player and the closest puzzle
        float distance = Vector3.Distance(player.transform.position, closestPuzzle);

        PuzzleGridHandler c = puzzleDict[closestPuzzle].GetComponent<PuzzleGridHandler>();

        Debug.Log(distance);
        Debug.Log(1f + ((c.width + c.height) / 2f * 0.639204f));

        if (c.plantRequired != "")
        {
            Transform pinepalm = reqCanvas.transform.Find("pinepalm");
            Transform pinepalmText = reqCanvas.transform.Find("pinepalmtext");

            pinepalm.gameObject.SetActive(true);
            pinepalmText.gameObject.SetActive(true);

            int pinepalmsCurrent = c.countType(c.plantRequired);

            pinepalmText.GetComponent<TMPro.TextMeshProUGUI>().text = pinepalmsCurrent.ToString() + "/" + c.plantRequiredAmount.ToString();

        }
        else
        {
            reqCanvas.transform.Find("pinepalm").gameObject.SetActive(false); 
            reqCanvas.transform.Find("pinepalmtext").gameObject.SetActive(false);
        }

        //If the player is close enough to the puzzle, display the requirements
        if (distance < 1f + ((c.width + c.height) / 2f * 0.639204f))
        {

            // Display the requirements
            reqCanvas.SetActive(true);

            int manaRequired = c.manaRequired;

            int manacurrent = c.CalculateMana();

            // set the text of the TMPro object to the plant name
            reqCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = manacurrent.ToString() + "/" + manaRequired.ToString();
        }
        else
        {
            // Hide the requirements
            reqCanvas.SetActive(false);
        }
    }

    public void Place(string plant)
    {

        Vector3 closestPuzzle = getClosest();

        PuzzleGridHandler c = puzzleDict[closestPuzzle].GetComponent<PuzzleGridHandler>();

        if (closestPuzzle != Vector3.zero)
        {
            c.GetComponent<PuzzleGridHandler>().Place(plant);
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
