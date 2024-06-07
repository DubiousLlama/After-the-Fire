using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaInstructions : MonoBehaviour
{
    public Transform player;
    // public Vector3 targetPosition = new Vector3(0.43,0.94,0);
    // public float tolerance = 0.5f;
    public GameObject hoverInstructionsCanvas;
    // Start is called before the first frame update
    void Start()
    {
        hoverInstructionsCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y >= 0.93){
            hoverInstructionsCanvas.SetActive(true);
        }else{
            hoverInstructionsCanvas.SetActive(false);
        }
    }
}
