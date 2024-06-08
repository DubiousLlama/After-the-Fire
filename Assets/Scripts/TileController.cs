using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    public TileBase[] newTiles;
    public BoundsInt totalArea;
    private Tilemap tiles;
    private Dictionary<string, TileBase> aTile;

    void Awake()
    {
        aTile = new Dictionary<string, TileBase>();
        tiles = transform.GetComponent<Tilemap>();
        foreach(TileBase tile in newTiles) {
            aTile.Add(tile.name, tile);
        }
    }

    // Start is called before the first frame update
    // 0,0 => 0
    // 1,0 => 1
    // 0,1 => 2
    // 1,1 => 3
    // 
    void Start()
    {
        // ActivateRiver(totalArea);
        // ActivateGrassPath(new BoundsInt(new Vector3Int(-1,-1,0), new Vector3Int(2,2,1)));
        // ActivateGrassPath(totalArea);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateRiver(BoundsInt area) {
        TileBase[] tileArray = tiles.GetTilesBlock(area);

        for(int y = area.y; y < area.yMax; y++) {
            for(int x = area.x; x < area.xMax; x++) {
                
                string newTile = tileArray[PointToIndex(area, x,y)].name + "a";

                if(aTile.ContainsKey(newTile))
                    tiles.SetTile(new Vector3Int(x, y), aTile[newTile]);
                // Debug.Log(aTile[tileArray[x + 2*y].name + "a"]);
                // Debug.Log(string.Format("x:{0} y:{1} idx:{2}", x, y, PointToIndex(x,y)));
            }
        }

    }

    public void ActivateGrassPath(BoundsInt area) {
        TileBase[] tileArray = tiles.GetTilesBlock(area);
        Debug.Log("hi kids");
        for(int y = area.y; y < area.yMax; y++) {
            for(int x = area.x; x < area.xMax; x++) {

                string newTile = tileArray[PointToIndex(area, x,y)].name + "b";

                if(aTile.ContainsKey(newTile))
                    tiles.SetTile(new Vector3Int(x, y), aTile[newTile]);
                // Debug.Log(aTile[tileArray[x + 2*y].name + "a"]);
                //Debug.Log(string.Format("x:{0} y:{1} idx:{2}", x, y, PointToIndex(x,y)));
            }
        }

    }


    private int PointToIndex(BoundsInt area, int x, int y) {
        Vector3Int size = area.size;

        return x - area.xMin + size.x*(y - area.yMin);
    }
}
