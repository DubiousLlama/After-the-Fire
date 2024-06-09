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

    public bool visible = false;

    // Start is called before the first frame update
    void Start()
    {
        pgh = GetComponent<PuzzleGridHandler>();
        // Find the canvas object in the scene
        canvas = GameObject.Find("ReqCanvas").GetComponent<Canvas>();
    }

    private void Update()
    {

        if (activeCanvas != null)
        {
            int manaRequired = pgh.manaRequired;

            int manacurrent = pgh.CalculateMana();

            // set the text of the TMPro object to the plant name
            activeCanvas.transform.Find("manatext").GetComponent<TMPro.TextMeshProUGUI>().text = manacurrent.ToString() + "/" + manaRequired.ToString();

            // If the puzzle requires a specific plant, update that as well
            if (pgh.plantRequired != "")
            {
                activeCanvas.transform.Find("pinepalmtext").GetComponent<TMPro.TextMeshProUGUI>().text = pgh.countType(pgh.plantRequired).ToString() + "/" + pgh.plantRequiredAmount.ToString();
            }
        }

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
                activeRect.anchoredPosition = new Vector2(Mathf.Lerp(activeRect.anchoredPosition.x, animationTarget, 0.07f), 0);
                if (Mathf.Abs(activeRect.anchoredPosition.x - animationTarget) < 20f)
                {
                    animating = false;
                    if (animationTarget == 200)
                    {
                        if (activeCanvas != null)
                        {
                            Destroy(activeCanvas);
                        }
                        activeCanvas = null;
                        activeRect = null;
                    }
                }
            }
        }
    }

    public void EnableGUI()
    {
        if (visible == true)
        {
            return;
        }

        activeCanvas = Instantiate(GUI, new Vector3(0, 0, 0), Quaternion.identity);
        activeRect = activeCanvas.GetComponent<RectTransform>();
        activeCanvas.transform.SetParent(canvas.transform, false);
        activeRect.anchoredPosition = new Vector2(200, 0);

        animating = true;
        visible = true;
        animationTarget = -20;
    }

    public void DisableGUI()
    {
        if (activeCanvas == null)
        {
            return;
        }
        if (visible == false)
        {
            return;
        }

        animating = true;
        visible = false;
        animationTarget = 200;

        Destroy(activeCanvas, 2f);
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
            visible = true;
            animationTarget = -20;
            return true;
        }
        else
        {
            animating = true;
            visible = false;
            animationTarget = 200;
            return true;
        }
    }
}
