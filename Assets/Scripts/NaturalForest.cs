using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalForest : MonoBehaviour
{
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

}
