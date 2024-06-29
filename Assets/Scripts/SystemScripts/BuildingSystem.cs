using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BuildingSystem : MonoBehaviour {

    [SerializeField] private TileBase highlightTile;
    [SerializeField] private Tilemap mainTilemap; //this will be our Building tilemap
    [SerializeField] private Tilemap tempTilemap;


    [SerializeField] private GameObject lootPrefab;

    private Vector3Int highlightedTilePos;
    private bool highlighted;
    private Vector3Int playerPos;



    void Update() {

        ItemScriptableObject item = InventoryManager.instance.GetSelectedItem(false);
        playerPos = mainTilemap.WorldToCell(transform.position);
        if (item != null)
        {
            HighlightTile(item);
        }

        if(Input.GetMouseButtonDown(0)) {
            if(highlighted) {
                if (item.item_type == itemType.Placeable) {
                    Build(highlightedTilePos, item);
                } else if (item.item_type == itemType.Pickaxe) {
                    Destroy(highlightedTilePos);
                }
            }
        }
    }

    private Vector3Int GetMouseOnGridPos()
    { //Calculates the grid position that the mouse is on
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCellPos = mainTilemap.WorldToCell(mousePos);
        mouseCellPos.z = 0;

        return mouseCellPos;
    }

    private void HighlightTile(ItemScriptableObject currentItem) {
        //get current mouse position on the grid
        Vector3Int mouseGridPos = GetMouseOnGridPos();

        //If that tile isn't the one already highlighted
        if (highlightedTilePos != mouseGridPos){
            //Erase the highlight on the currently highlighted tile
            print("ERASING HIGHLIGHt");
            tempTilemap.SetTile(highlightedTilePos, null);

            //Get the new tile
            //TileBase tile = mainTilemap.GetTile(mouseGridPos);

            if(InRange(playerPos, mouseGridPos, (Vector3Int)currentItem.range)) {
                if (CheckCondition(mainTilemap.GetTile<RuleTileWithData>(mouseGridPos), currentItem)){
                    //If the tile exists, highlight it
                    //In the future, highlightTile should be the preview of whatever we're holding.
                    tempTilemap.SetTile(mouseGridPos, highlightTile);
                    highlightedTilePos = mouseGridPos;
                    print("HIGHLIGHTING");
                    highlighted = true;
                } else {
                    print("highlight=false IN IF");
                    highlighted = false;
                    highlightedTilePos = new Vector3Int(-999, 999, 0);
                }
            } else {
                highlighted = false;
            }

        }
    }

    private bool InRange(Vector3Int positionA, Vector3Int positionB, Vector3Int range) {
        Vector3Int distance = positionA - positionB;
        return !(Mathf.Abs(distance.x) >= range.x || Mathf.Abs(distance.y) >= range.y);
    }

    private bool CheckCondition(RuleTileWithData tile, ItemScriptableObject currentItem) {
        //if the current item we are holding is a placeable object
        if (currentItem.item_type == itemType.Placeable) {
            //if the tile does not exist (aka its empty)
            if (!tile) {
                //return true because we should highlight it, since it's a placeable position
                return true;
            }
        }
        //else if our current item is a pickaxe
        else if (currentItem.item_type == itemType.Pickaxe) {
            //and the tile exists
            if(tile) {
                //and the tile's action type is equal to our item action type (mineable, diggable, etc)
                if (tile.item.action_type == currentItem.action_type) {
                    return true;
                }
            }
            
        }
        return false;
    }

    private void Build(Vector3Int position, ItemScriptableObject itemToBuild) {
        if(!IsPointerOverUI()) {
            tempTilemap.SetTile(position, null); //clear the highlihgt
            highlighted = false;

            InventoryManager.instance.GetSelectedItem(true);

            mainTilemap.SetTile(position, itemToBuild.tile);
        }
    }

    private void Destroy(Vector3Int position) {
        if(!IsPointerOverUI()) {
            tempTilemap.SetTile(position, null); //clear the highlihgt
            highlighted = false;

            RuleTileWithData tile = mainTilemap.GetTile<RuleTileWithData>(position);
            mainTilemap.SetTile(position, null); //clear the actual tile

            Vector3 pos = mainTilemap.GetCellCenterWorld(position);
            GameObject loot = Instantiate(lootPrefab, pos, Quaternion.identity);
            loot.GetComponent<Loot>().Initialize(tile.item);
        }
    }

    private bool IsPointerOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
