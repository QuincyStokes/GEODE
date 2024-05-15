using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("General")]

    public List<itemType> inventoryList;
    public int selectedItem;
    public float playerReach;

    [Space(20)]
    [Header("Keys")]
    [SerializeField] KeyCode throwItemKey;
    //[SerializeField] KeyCode pickItemKey;

    [Space(20)]
    [Header("Item gameobjects")]
    [SerializeField] GameObject pickaxe_item;
    [SerializeField] GameObject sword_item;
    [SerializeField] GameObject bow_item;

    [Space(20)]
    [Header("Item Prefabs")]
    [SerializeField] GameObject pickaxe_prefab;
    [SerializeField] GameObject sword_prefab;
    [SerializeField] GameObject bow_prefab;

    [Space(20)]
    [Header("UI")]
    [SerializeField] Image[] inventorySlotImage = new Image[9];
    [SerializeField] Image[] inventoryBackgroundImage = new Image[9];
    [SerializeField] Sprite emptySlotSprite;

    [SerializeField] Camera cam;

    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };
    private Dictionary<itemType, GameObject> itemInstantiate = new Dictionary<itemType, GameObject>() { };


    void Start() {
        itemSetActive.Add(itemType.Pickaxe, pickaxe_item);
        itemSetActive.Add(itemType.Sword, sword_item);
        itemSetActive.Add(itemType.Bow, bow_item);   

        itemInstantiate.Add(itemType.Pickaxe, pickaxe_prefab);
        itemInstantiate.Add(itemType.Sword, sword_prefab);
        itemInstantiate.Add(itemType.Bow, bow_prefab);   

        NewItemSelected();
    }

    void Update() {
        /*
        Ray ray = cam.ScreenPointToRay(transform.position);
        RaycastHit hitInfo;
        Debug.Log(transform.position, transform.TransformDirection(Vector3.forward));
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, playerReach)) {
            Debug.Log("We did something!");
            IPickable item = hitInfo.collider.GetComponent<IPickable>();
            if(item != null) {
                Debug.Log("Item picked u");
                inventoryList.Add(hitInfo.collider.GetComponent<ItemPickable>().itemScriptableObject.item_type);
                item.PickItem();
            }
        }
        */
        //just make a "pickup hitbox circle", if an item collides with it, yoink it


        //Throw item

        if(Input.GetKeyDown(throwItemKey) && inventoryList.Count > 0) {
            GameObject droppedItem = Instantiate(itemInstantiate[inventoryList[selectedItem]], position: transform.position, new Quaternion());
            Debug.Log("Removed item " + selectedItem);
           
            ItemPickable itemPickable = droppedItem.GetComponent<ItemPickable>();
            if (itemPickable != null) {
                itemPickable.DropItem();
            }
            inventoryList.RemoveAt(selectedItem);

            if(selectedItem != 0) {
                selectedItem -= 1;
            }
            NewItemSelected();
        }

        //UI
        for (int i = 0; i < 9; ++i) {
            if (i < inventoryList.Count) {

                if (inventorySlotImage[i] == null) {
                    Debug.LogError($"inventorySlotImage[{i}] is null");
                    continue;
                }

                var item = itemSetActive[inventoryList[i]];
                if (item == null) {
                    Debug.LogError($"itemSetActive for {inventoryList[i]} is null");
                    continue;
                }

                var itemComponent = item.GetComponent<ItemPickable>();
                if (itemComponent == null) {
                    Debug.LogError($"Item component not found on {item.name}");
                    continue;
                }

                var itemSprite = itemComponent.itemScriptableObject?.item_sprite;
                if (itemSprite == null) {
                    Debug.LogError($"Item sprite is null for {itemComponent.itemScriptableObject.name}");
                    continue;
                }
                
                inventorySlotImage[i].sprite = itemSetActive[inventoryList[i]].GetComponent<ItemPickable>().itemScriptableObject.item_sprite;
            } else {
                inventorySlotImage[i].sprite = emptySlotSprite;
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedItem = 0;
            NewItemSelected();
        }
         if (Input.GetKeyDown(KeyCode.Alpha2)) {
            selectedItem = 1;
            NewItemSelected();
        }
         if (Input.GetKeyDown(KeyCode.Alpha3)) {
            selectedItem = 2;
            NewItemSelected();
        }
         if (Input.GetKeyDown(KeyCode.Alpha4)) {
            selectedItem = 3;
            NewItemSelected();
        }
         if (Input.GetKeyDown(KeyCode.Alpha5)) {
            selectedItem = 4;
            NewItemSelected();
        }
         if (Input.GetKeyDown(KeyCode.Alpha6)) {
            selectedItem = 5;
            NewItemSelected();
        }
         if (Input.GetKeyDown(KeyCode.Alpha7)) {
            selectedItem = 6;
            NewItemSelected();
        }
         if (Input.GetKeyDown(KeyCode.Alpha8)) {
            selectedItem = 7;
            NewItemSelected();
        }
         if (Input.GetKeyDown(KeyCode.Alpha9)) {
            selectedItem = 8;
            NewItemSelected();
        }
    }
    
    private void NewItemSelected() {
        pickaxe_item.SetActive(false);
        sword_item.SetActive(false);
        bow_item.SetActive(false);

        GameObject selectedItemGameobject = itemSetActive[inventoryList[selectedItem]];
        selectedItemGameobject.SetActive(true);
    }
//
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Pickupable")) {
            
           
            Debug.Log("Time Difference:" + (Time.time - other.GetComponent<ItemPickable>().itemScriptableObject.lastTimeDropped).ToString());

            IPickable item = other.GetComponent<ItemPickable>();
            if(item != null) {
                if(Time.time - other.GetComponent<ItemPickable>().itemScriptableObject.lastTimeDropped > other.GetComponent<ItemPickable>().itemScriptableObject.pickupCooldown) {
                inventoryList.Add(other.GetComponent<ItemPickable>().itemScriptableObject.item_type);
                //inventoryList.Add(item.ItemScriptableObject);
                Debug.Log("ITEM PICKED UP");
                item.PickItem();
            } else {
                //other.GetComponent<ItemPickable>().itemScriptableObject.lastTimeDropped = Time.time;
            }
            }
           
            
        }
    }

}

public interface IPickable {
    void PickItem();
}
