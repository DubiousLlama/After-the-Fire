using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;



public class MovementScript : MonoBehaviour
{
    GridHandler.PuzzleGridHandler gridHandler;

    // Start is called before the first frame update
    void Start()
    {

        // Get the grid handler
        gridHandler = GameObject.Find("Puzzle Grid Handler").GetComponent<PuzzleGridHandler>();


    }

    private void Update()
    {
        if (gridHandler == null)
        {
            Debug.LogError("Grid handler not found");
            return;
        }

        // If the space bar is pressed, place a plant
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            // Place a plant at the grid position
            gridHandler.Place("moonglow");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            // Place a plant at the grid position
            gridHandler.Place("starleaf tree");
        }
    }

    // Write a basic movement script that moves the object in the direction of the arrow keys
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 0.1f, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, -0.1f, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(0.1f, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-0.1f, 0, 0);
        }
    }
}
