using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalForest : MonoBehaviour
{
    public Sprite happyTree;

    // Start is called before the first frame update
    void Start()
    {
        // Get all children of the game object
        Transform[] children = GetComponentsInChildren<Transform>();

        // Loop through all children
        foreach (Transform child in children)
        {
            // Offset the transform's position by a small x and y amount
            child.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
        }
    }

    public void ActivateTrees(BoundsInt area) {
        foreach (Transform child in transform)
        {
            Vector3 pos = child.position;
            if(pos.x >= area.xMin && pos.x < area.xMax && pos.y >= area.yMin && pos.y < area.yMax)
                child.GetComponent<SpriteRenderer>().sprite = happyTree;
        }

    }

}
