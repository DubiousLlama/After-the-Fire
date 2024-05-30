using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas interactionsCanvas; 
    void Start()
    {
        if (interactionsCanvas != null){
            interactionsCanvas.enabled = false;
            Debug.Log("canvas found");
        }else{
            Debug.Log("canvas not found");
        }
    }
    void OnMouseDown()
    {
        // When the object is clicked, show the dialogue box
        Debug.Log("Clicked");
        if (interactionsCanvas != null)
        {
            Debug.Log("ENABLED");
            interactionsCanvas.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
