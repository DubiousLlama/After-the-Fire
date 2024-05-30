using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;
using System.ComponentModel.Design;
using System;
// using System.Numerics;



public class MovementScript : MonoBehaviour
{
    PuzzleManager gridHandler;
    
    [SerializeField]
    private BoxCollider2D z_BoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        // Get the grid handler
        gridHandler = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
        z_BoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (gridHandler == null)
        {
            Debug.LogError("Grid handler not found");
            return;
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

        float moveX =  Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

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

        //check collision X
        RaycastHit2D castResult = Physics2D.BoxCast(transform.position, 
                                                    z_BoxCollider.size, 
                                                    0, 
                                                    new Vector2(moveX, 0),
                                                    Mathf.Abs(moveX * Time.fixedDeltaTime),
                                                    LayerMask.GetMask("BlockMove")
        );

        if(castResult.collider) {
            //stop moving X
            Debug.Log("stop x movement!");
        }

        //check collision Y
        castResult = Physics2D.BoxCast(transform.position, 
                                                    z_BoxCollider.size, 
                                                    0, 
                                                    new Vector2(moveY, 0),
                                                    Mathf.Abs(moveY * Time.fixedDeltaTime),
                                                    LayerMask.GetMask("BlockMove")
        );

        if(castResult.collider) {
            //stop moving Y
            Debug.Log("stop y movement!");
        }

    }
}
