using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;
using TMPro;

public class DisplayManaOnHover : MonoBehaviour
{
    PuzzleGridHandler[] puzzleGridHandlers;

    GameObject display = null;

    public GameObject manaDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // Find all the PuzzleGridHandler objects in the scene
        puzzleGridHandlers = FindObjectsOfType<PuzzleGridHandler>();
        Debug.Log(puzzleGridHandlers.Length);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Update");

        // get the x and y location of the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 gridPos = new Vector2(-1, -1);
        Vector2 notInGrid = new Vector2(-1, -1);
        PuzzleGridHandler currentPuzzle = puzzleGridHandlers[0];

        GameObject player = GameObject.Find("Player");

        // Get the position of the player and print it to the debug log
        // Debug.Log("Player position: " + player.transform.position + ", Mouse position: " + mousePos);
        //Debug.Log("Player position: " + puzzleGridHandlers[0].PositionToTile(player.transform.position) + ", Mouse position: " + puzzleGridHandlers[0].PositionToTile(mousePos));

        // translate that to a grid location
        foreach (PuzzleGridHandler puzzleGridHandler in puzzleGridHandlers)
        {
            Vector2 loc = puzzleGridHandler.PositionToTile(mousePos);


            if (loc != notInGrid)
            {
                gridPos = loc;
                currentPuzzle = puzzleGridHandler;
            }
        }

        if (gridPos == notInGrid)
        {
            if (display != null)
            {
                Destroy(display);
            }

            // Debug.Log("Cursor at " + mousePos.ToString() + " not in grid, returns " + gridPos);
            display = null;
        }
        else
        {
            // get the mana at that location
            int mana = currentPuzzle.calculateManaAtPosition((int)gridPos.y, (int)gridPos.x);

            // Find the center point 
            Vector3 center = currentPuzzle.tiles[(int)gridPos.y, (int)gridPos.x].transform.position;

            this.transform.position = center;

            // display the ManaDisplay
            // Debug.Log("Displaying mana at " + center);
            if (display == null)
            {
                display = Instantiate(manaDisplay, Vector3.zero, Quaternion.identity);

                // Instantiate the ManaDisplay as the child of the Canvas this script is attached to
                display.transform.SetParent(this.transform);
                display.transform.localPosition = new Vector3(0, 0, -1.5f);

            }

            // Disable the display if the hovered tile does not have a plantable tag
            if (display != null)
            {
                if (!(currentPuzzle.tiles[(int)gridPos.y, (int)gridPos.x].tag.Equals("Plantable") || currentPuzzle.tiles[(int)gridPos.y, (int)gridPos.x].tag.Equals("2x")))
                {
                    display.SetActive(false);
                }
                else
                {
                    display.SetActive(true);
                }
            }
           
            // set the text of the ManaDisplay to the mana at that location
            display.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mana.ToString();
        }


    }
}

