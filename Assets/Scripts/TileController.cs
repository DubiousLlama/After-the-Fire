using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    public AnimatedTile newTile;
    private Tilemap tiles;


    void Awake()
    {
        tiles = transform.GetComponent<Tilemap>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3Int test = new Vector3Int(1, 2);
        tiles.SetTile(new Vector3Int(0,0), newTile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
