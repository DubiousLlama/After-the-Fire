using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;



public class MovementScript : MonoBehaviour
{
    PuzzleManager gridHandler;

    // Start is called before the first frame update
    void Start()
    {

        // Get the grid handler
        gridHandler = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();


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
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            // Place a plant at the grid position
            gridHandler.Place("pinepalm");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

            // Place a plant at the grid position
            gridHandler.Place("bloomberry");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {

            // Place a plant at the grid position
            gridHandler.Place("soil");
        }
    }

    // Write a basic movement script that moves the object in the direction of the arrow keys
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0.08f, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -0.08f, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0.08f, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-0.08f, 0, 0);
        }
    }
}
