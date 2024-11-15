using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator instance;
    

    [SerializeField]
    public int gridSizeX = 100;
    [SerializeField]
    public int gridSizeY = 100;
    public TileBase[] floorTiles;
    public Tilemap tilemap;
    

 void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

    }
    void Start()
    {
        GenerateGrid();
    }

    

    void GenerateGrid(){
        int startX = 0 - gridSizeX/2;
        int startY = 0 - gridSizeY/2;
        for (int x = startX; x < gridSizeX/2; x++) {
            for (int y = startY; y < gridSizeY/2; y++) {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                tilemap.SetTile(cellPosition, floorTiles[Random.Range(0, floorTiles.Length)]);
            }
        }
    }
}
   
