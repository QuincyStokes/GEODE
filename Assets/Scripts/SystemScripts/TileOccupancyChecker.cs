using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOccupancyChecker : MonoBehaviour
{
    // Start is called before the first frame update

    public static TileOccupancyChecker instance;
    public Grid grid;
    public LayerMask buildingLayer;

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
        if (grid == null) {
            grid = FindObjectOfType<Grid>();
        }
    }

    public bool IsWorldPosOccupied(Vector3 worldposition) {
        //turn the world position into a grid position
        Vector3Int cellPosition = grid.WorldToCell(worldposition);
        //Get the center of the grid as world coordinates
        Vector3 cellCenter = grid.GetCellCenterWorld(cellPosition);
        print("CHECKING POSITION AT ");
        print(cellCenter);
        //Grab the collider at THAT world point
        Collider2D collider = Physics2D.OverlapPoint(cellCenter, buildingLayer);
        //if the collider is null, return false because tile is not occupied
        //if the collider HAS something, then return true because it's occupied
        print("Is this tile occupied?");
        print(collider != null);
        return collider != null;
    }

}
