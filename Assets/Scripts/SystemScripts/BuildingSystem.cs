using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BuildingSystem : MonoBehaviour {

    [SerializeField] private TileBase highlightTile;
    //[SerializeField] private Tilemap buildingTilemap; //this will be our Building tilemap
    [SerializeField] private Tilemap highlightTilemap;
    [SerializeField] private Tilemap objectTilemap;
    [SerializeField] private TileOccupancyChecker tileOccupancyChecker;


    [SerializeField] private GameObject lootPrefab;

    private Vector3Int highlightedTilePos;
    private bool highlighted;
    private Vector3Int playerPos;
    [SerializeField] private Grid grid;
    [SerializeField] private LayerMask layermask;



    void Update() {

        ItemScriptableObject item = InventoryManager.instance.GetSelectedItem(false);
        ItemManager.instance.currentItem = item;
        playerPos = highlightTilemap.WorldToCell(transform.position);
        //if we're holding something, anything, try to highlight it
        if (item != null)
        {
            HighlightTile(item);
        }

        if(Input.GetMouseButtonDown(0)) {
            if(highlighted) {
                if (item.item_type == itemType.Placeable) {
                    Build(highlightedTilePos, item);
                //} else {
                } else if (item.item_type == itemType.Hammer) {
                    DestroyGameObject(highlightedTilePos);
                }
                //if type is a "tower?"
                    //place tower gameobject
            }
        }
    }

    private Vector3Int GetMouseOnGridPos()
    { //Calculates the grid position that the mouse is on
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCellPos = grid.WorldToCell(mousePos);
        mouseCellPos.z = 0;

        return mouseCellPos;
    }

    private void HighlightTile(ItemScriptableObject currentItem) {
        //get current mouse position on the grid
        Vector3Int mouseGridPos = GetMouseOnGridPos();

        //If that tile isn't the one already highlighted
        if (highlightedTilePos != mouseGridPos){
            //Erase the highlight on the currently highlighted tile
            highlightTilemap.SetTile(highlightedTilePos, null);

            //Get the new tile
            //TileBase tile = buildingTilemap.GetTile(mouseGridPos);

            if(InRange(playerPos, mouseGridPos, currentItem.range)) { //if we're in range
                Collider2D collider = Physics2D.OverlapPoint(grid.GetCellCenterWorld(mouseGridPos), layermask);
               
                if(CheckConditionGO(collider, currentItem)) {
                //if (CheckCondition(buildingTilemap.GetTile<RuleTileWithData>(mouseGridPos), currentItem)){ //if the tile hovered's type matches the item we're holding
                    //if(!tileOccupancyChecker.IsWorldPosOccupied(grid.GetCellCenterWorld(mouseGridPos))){//If the tile is occupied already
                        //In the future, highlight Tile should be the preview of whatever we're holding.
                        highlightTilemap.SetTile(mouseGridPos, highlightTile);
                        highlightedTilePos = mouseGridPos;
                        highlighted = true;
                    //} else {
                       
                     
                } else {
                    highlighted = false;
                    highlightedTilePos = new Vector3Int(-999, 999, 0); //should probably change this lmao
                }
            } else {
                highlighted = false;
            }

        }
    }
    

    private bool InRange(Vector3Int positionA, Vector3Int positionB, int range) {
        return (positionA - positionB).sqrMagnitude <= range * range;
    }
    private bool CheckConditionGO(Collider2D collider, ItemScriptableObject currentItem) {
        //if we're holding a placeable item and there is no collider at current location, it's a placeable spot

        //cases in which we highlight an object:
            //if we're holding a placeable object, and the tile is null
            //if we're holding a hammer, and the tile in question is of action_type, hammer
            //thats all for now I think..
        if(collider == null) {
        }
        if(currentItem.item_type == itemType.Placeable && collider == null) {
            return true;
            //if we're holding a pickaxe and there *is* a collider
        } else if (currentItem.item_type == itemType.Hammer && collider != null) {
        
            DestructableScript des = collider.GetComponent<DestructableScript>();
           
            //if the item's action_type matches the gameobject's action_type, we can highlight it
            if(des.action_type == currentItem.action_type) {
                return true;
            }
        } 
        return false;
    }
    private bool CheckCondition(RuleTileWithData buildTile, ItemScriptableObject currentItem) {
        //if the current item we are holding is a placeable object
        if (currentItem.item_type == itemType.Placeable) {
            //if the tile does not exist (aka its empty) and the tile isnt occupied by a gameobject
            if (!buildTile) {
                //return true because we should highlight it, since it's a placeable position
                return true;
            }
        }
        //else if our current item is a pickaxe
        else if (currentItem.item_type == itemType.Pickaxe) {
            //and the buildTile exists
            if(buildTile) {
                //and the tile's action type is equal to our item action type (mineable, diggable, etc)
                if (buildTile.action_type == currentItem.action_type) {
                    return true;
                }
            } 
            
        } else if (currentItem.item_type == itemType.Axe) {
            if(buildTile) {
                //and the tile's action type is equal to our item action type (mineable, diggable, etc)
                if (buildTile.action_type == currentItem.action_type) {
                    return true;
                }
            } 
        }
        return false;
    }

    private void Build(Vector3Int position, ItemScriptableObject itemToBuild) {
        Vector3 cellPositionAsWorld = grid.GetCellCenterWorld(position);
        if(!IsPointerOverUI() && !tileOccupancyChecker.IsWorldPosOccupied(cellPositionAsWorld)) {
            highlightTilemap.SetTile(position, null); //clear the highlihgt
            highlighted = false;
            
            InventoryManager.instance.GetSelectedItem(true);
            //buildingTilemap.SetTile(position, itemToBuild.tile);
            Instantiate(itemToBuild.item_prefab, cellPositionAsWorld, Quaternion.identity);
            NavMeshManager.instance.UpdateNevMesh();



/*
//first get position in the world
                    Vector3Int worldPosition = new Vector3Int(x, y, 0);
                    //change it to position on the grid
                    Vector3Int cellPosition = grid.WorldToCell(worldPosition);
                    //then change it to the center of that grid, this is where we'll place the object.
                    Vector3 cellCenter = grid.GetCellCenterWorld(cellPosition);
                    //Vector3 cellCenter = grid.GetCellCenterWorld(cellPosition);
                    int index = Random.Range(0, desertTrees.Length);
                    print("Instantiating at cell coordinates " + cellCenter);
                    Instantiate(desertTrees[index], cellCenter, Quaternion.identity);
*/
        }
    }

    private void DestroyGameObject(Vector3Int position) {
        if(!IsPointerOverUI()) {
           //this is grid coordinates, not world coordinates
            Vector3 worldPos = grid.GetCellCenterWorld(position);
            print(worldPos);
            Collider2D collider = Physics2D.OverlapPoint(worldPos, layermask);
            if(collider != null) {
                DestructableScript des = collider.GetComponent<DestructableScript>();
                des.TakeDamage(des.maxHealth);
            } else {
                print("Cannot Destroy GO, no collider found.");
            }
            
        }
    }

    private bool IsPointerOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
