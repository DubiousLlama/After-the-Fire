using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;
using System.ComponentModel.Design;
using System;
using JetBrains.Annotations;

public class AnimatedMovement : MonoBehaviour
{
    PuzzleManager gridHandler;
    InventoryManager invManager;

    [SerializeField]
    private BoxCollider2D z_BoxCollider;
    private bool inDialogue;

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;  // Using Animator instead of SpriteRenderer

    Vector2 movement;
    Vector2 lastMovement; // Store the last movement direction

    void Awake() 
    {
        inDialogue = false;
    }

    void Start()
    {
        // Get the grid handler
        gridHandler = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
        invManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        z_BoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Prevent diagonal movement by prioritizing horizontal movement
        if (movement.x != 0)
        {
            movement.y = 0;
        }

        if (movement.x != 0 || movement.y != 0)
        {
            lastMovement = movement; // Update last movement direction
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.sqrMagnitude == 0)
        {
            animator.SetFloat("LastMoveX", lastMovement.x);
            animator.SetFloat("LastMoveY", lastMovement.y);
        }

        // Inventory controls
        if (!inDialogue && Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8)
            {
                invManager.ChangeSelectedSlot(number - 1);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !invManager.puzzleGrid.checkSolved())
            {
                string plantedPlant = invManager.puzzleGrid.getContent();
                if (plantedPlant != "out of bounds" && plantedPlant == "soil")
                {
                    Item item = invManager.GetSelectedItem(false);
                    if (item != null && invManager.puzzleGrid.Place(item.name))
                    {
                        Debug.Log("About to call GetSelectedItem to pick up plant " + item.name);
                        invManager.GetSelectedItem(true);
                        Debug.Log("Finished calling GetSelectedItem on" + item.name);
                    }
                }
                else if (plantedPlant != "out of bounds" && plantedPlant != "soil" && plantedPlant != "infertileSoil")
                {
                    Item item = invManager.GetSelectedItem(false);
                    if (item != null)
                    {
                        if (plantedPlant != item.name)
                        {
                            invManager.puzzleGrid.Place(item.name);
                            invManager.GetSelectedItem(true);
                            invManager.AddItem(plantedPlant);
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.R) && !invManager.puzzleGrid.checkSolved())
            {
                string plantedPlant = invManager.puzzleGrid.getContent();
                if (plantedPlant != "out of bounds" && plantedPlant != "soil")
                {
                    invManager.puzzleGrid.Place("soil");
                    invManager.AddItem(plantedPlant);
                }
            }
        }
    }

    void FixedUpdate() 
    {
        if (!inDialogue) 
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            CheckCollision();
        }
    }

    void CheckCollision() 
    {
        float moveX = movement.x;
        float moveY = movement.y;

        // Check collision X
        RaycastHit2D castResult = Physics2D.BoxCast(transform.position, 
                                                    z_BoxCollider.size, 
                                                    0, 
                                                    new Vector2(moveX, 0),
                                                    Mathf.Abs(moveX * Time.fixedDeltaTime),
                                                    LayerMask.GetMask("BlockMove")
        );

        if (castResult.collider) 
        {
            // Stop moving X
            
        }

        // Check collision Y
        castResult = Physics2D.BoxCast(transform.position, 
                                                    z_BoxCollider.size, 
                                                    0, 
                                                    new Vector2(moveY, 0),
                                                    Mathf.Abs(moveY * Time.fixedDeltaTime),
                                                    LayerMask.GetMask("BlockMove")
        );

        if (castResult.collider) 
        {
            // Stop moving Y
            
        }
    }

    public void setDialogueState(bool state) 
    {
        inDialogue = state;
    }

    public bool getDialogueState()
    {
        return inDialogue;
    }
}
