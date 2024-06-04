using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;
using System.ComponentModel.Design;
using System;

public class AnimatedMovement : MonoBehaviour
{

    PuzzleManager gridHandler;

    [SerializeField]
    private BoxCollider2D z_BoxCollider;

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    Vector2 movement;


    void Start()
    {
        // Get the grid handler
        gridHandler = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
        z_BoxCollider = GetComponent<BoxCollider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        UpdateSprite();
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        CheckCollision();
    }

    void UpdateSprite()
    {
        if (movement.x > 0)
        {
            spriteRenderer.sprite = spriteRight;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.sprite = spriteLeft;
        }
        else if (movement.y > 0)
        {
            spriteRenderer.sprite = spriteUp;
        }
        else if (movement.y < 0)
        {
            spriteRenderer.sprite = spriteDown;
        }
    }

    void CheckCollision() {
        float moveX = movement.x;
        float moveY = movement.y;

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
