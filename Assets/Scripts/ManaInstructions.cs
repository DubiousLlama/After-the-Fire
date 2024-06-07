using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaInstructions : MonoBehaviour
{
    public Transform player;
    // public Vector3 targetPosition = new Vector3(0.43,0.94,0);
    // public float tolerance = 0.5f;
    public GameObject numbersInstructionsCanvas;
    private bool numbersInstructOver;
    // private bool showPlantingInstructions;
    public GameObject hoverInstructionsCanvas;
    // Start is called before the first frame update
    void Start()
    {
        numbersInstructionsCanvas.SetActive(false);
        hoverInstructionsCanvas.SetActive(false);
        numbersInstructOver = false;
        // showPlantingInstructions = true; 
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y >= 0.93){
            if (!numbersInstructOver){
                numbersInstructionsCanvas.SetActive(true);
            }
           
            // showPlantingInstructions = false; 
            if (Input.GetKeyDown(KeyCode.Space)){
                numbersInstructOver = true;
                numbersInstructionsCanvas.SetActive(false);
                hoverInstructionsCanvas.SetActive(true);
            }
        }else{
            numbersInstructionsCanvas.SetActive(false);
            hoverInstructionsCanvas.SetActive(false);
        }
        
    }
}
