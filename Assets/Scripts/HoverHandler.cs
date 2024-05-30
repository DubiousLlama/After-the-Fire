using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverHandler : MonoBehaviour
{
    public GameObject hoverPanel;
    public Camera cam;

    void OnMouseOver()
    {
        if (hoverPanel != null){
            Debug.Log("We hovering");
            hoverPanel.SetActive(true);
            hoverPanel.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
            
        }
    }
    void OnMouseExit()
    {
        Debug.Log("We not hovering");
        if (hoverPanel != null){
            hoverPanel.SetActive(false);
        }
    }
}
