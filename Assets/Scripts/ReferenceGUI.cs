using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridHandler;

public class ReferenceGUI : MonoBehaviour
{

    // Get the PuzzleGridHandler script attached to the same game object
    PuzzleGridHandler pgh;

    public GameObject reqCanvas;

    GameObject activeCanvas = null;

    bool animating = false;

    // Start is called before the first frame update
    void Start()
    {
        pgh = GetComponent<PuzzleGridHandler>();
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
            activeCanvas = Instantiate(reqCanvas, new Vector3(0, 0, 0), Quaternion.identity);
            activeCanvas.transform.GetChild(0).transform.position = new Vector3(200, 0, 0);
            animating = true;
            StartCoroutine(slideIn(activeCanvas));
            return true;
        }
        else
        {
            animating = true;
            StartCoroutine(slideOut(activeCanvas));
            return true;
        } 
    }

    private IEnumerator slideIn(GameObject reqCanvas, float speed=16.0f)
    {
        Transform position = activeCanvas.transform.GetChild(0).transform;
        while (position.position.x > 1)
        {
            position.position = new Vector3(expDecay(position.position.x, 0, speed, Time.deltaTime), 0, 0);
            yield return null;
        }
        position.position = new Vector3(0, 0, 0);
        animating = false;
        yield return null;
    }

    private IEnumerator slideOut(GameObject reqCanvas, float speed = 16.0f)
    {
        Transform position = activeCanvas.transform.GetChild(0).transform;
        while (position.position.x < 180)
        {
            position.position = new Vector3(expDecay(position.position.x, 180, speed, Time.deltaTime), 0, 0);
            yield return null;
        }
        Destroy(activeCanvas);
        activeCanvas = null;
        animating = false;
        yield return null;
    }

    private float expDecay(float a, float b, float decay, float dt)
    {
        return a + (b - a) * Mathf.Exp(-decay * dt);
    }


}
