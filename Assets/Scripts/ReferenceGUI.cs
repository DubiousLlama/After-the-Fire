using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;
using Unity.VisualScripting;

public class ReferenceGUI : MonoBehaviour
{

    // Get the PuzzleGridHandler script attached to the same game object
    PuzzleGridHandler pgh;

    public GameObject GUI;

    Canvas canvas;

    GameObject activeCanvas = null;

    RectTransform activeRect = null;

    bool animating = false;

    float animationTarget;

    // Start is called before the first frame update
    void Start()
    {
        pgh = GetComponent<PuzzleGridHandler>();
        // Find the canvas object in the scene
        canvas = GameObject.Find("ReqCanvas").GetComponent<Canvas>();
        toggleGUI();
    }

    // Update is called once per frame
    void Update()
    {
        // When the player presses the "E" key, toggle the GUI
        if (Input.GetKeyDown(KeyCode.E))
        {
            toggleGUI();
        }

        //if (activeCanvas != null)
        //{
        //    if (activeRect == null)
        //    {
        //        activeRect = activeCanvas.GetComponent<RectTransform>();
        //    }
        //    if (animating)
        //    {
        //        activeRect.anchoredPosition = new Vector2((float)expDecay(activeRect.anchoredPosition.x, animationTarget, 2000f, Time.deltaTime), 0);
        //        Debug.Log("X pos: " + activeRect.anchoredPosition.x.ToString());
        //        if (Mathf.Abs(activeRect.anchoredPosition.x - animationTarget) < 5f)
        //        {
        //            animating = false;
        //            if (animationTarget == 200)
        //            {
        //                Destroy(activeCanvas);
        //                activeCanvas = null;
        //                activeRect = null;
        //            }
        //        }
        //    }
        //}

    }

    private void FixedUpdate()
    {
        if (activeCanvas != null)
        {
            if (activeRect == null)
            {
                activeRect = activeCanvas.GetComponent<RectTransform>();
            }
            if (animating)
            {
                activeRect.anchoredPosition = new Vector2(Mathf.Lerp(activeRect.anchoredPosition.x, animationTarget, 0.05f), 0);
                Debug.Log("X pos: " + activeRect.anchoredPosition.x.ToString());
                if (Mathf.Abs(activeRect.anchoredPosition.x - animationTarget) < 20f)
                {
                    animating = false;
                    if (animationTarget == 200)
                    {
                        Destroy(activeCanvas);
                        activeCanvas = null;
                        activeRect = null;
                    }
                }
            }
        }
    }

    // If the GUI is not active, enable it and have it slide in from the right
    // If the GUI is active, disable it and have it slide out to the right
    public bool toggleGUI()
    {
        if (animating)
        {
            return false;
        }

        if (activeCanvas == null)
        {
            activeCanvas = Instantiate(GUI, new Vector3(0, 0, 0), Quaternion.identity);
            activeRect = activeCanvas.GetComponent<RectTransform>();
            activeCanvas.transform.SetParent(canvas.transform, false);
            activeRect.anchoredPosition = new Vector2(200, 0);

            animating = true;
            animationTarget = -20;
            return true;
        }
        else
        {
            animating = true;
            animationTarget = 200;
            return true;
        } 
    }

    private double expDecay(double a, double b, double decay, double dt)
    {
        return a + (b - a) * System.Math.Exp(-decay * dt);
    }


}
