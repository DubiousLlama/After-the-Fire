using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToLevelOne : MonoBehaviour
{
    public GameObject vine_wall;
    AnimatedMovement movementScript;
    SceneFader sceneFader;

    private void Start()
    {
        movementScript = GameObject.Find("Player").GetComponent<AnimatedMovement>();
        sceneFader = GameObject.Find("FadeCanvas").GetComponent<SceneFader>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.tag == "Player" && !vine_wall.activeSelf && !movementScript.getDialogueState())
        {
            sceneFader.FadeToScene("Level One");
        }
    }
}

