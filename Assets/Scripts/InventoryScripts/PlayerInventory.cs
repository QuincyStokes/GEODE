using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("General")]

    public Dictionary<int, itemType> playerInventory = new Dictionary<int, itemType>();

    //public List<itemType> playerInventory;
    public int selectedItem;
    public float playerReach;

    [Space(20)]
    [Header("Keys")]
    [SerializeField] KeyCode throwItemKey;
    //[SerializeField] KeyCode pickItemKey;
    [SerializeField] KeyCode openInventoryKey;

    [Space(20)]
    [Header("Item gameobjects")]
    //[SerializeField] GameObject null_item;
    [SerializeField] GameObject pickaxe_item;
    [SerializeField] GameObject sword_item;
    [SerializeField] GameObject bow_item;

    [Space(20)]
    [Header("Item Prefabs")]
    //might need a null prefab?
    [SerializeField] GameObject pickaxe_prefab;
    [SerializeField] GameObject sword_prefab;
    [SerializeField] GameObject bow_prefab;

    [Space(20)]
    [Header("UI")]
    [SerializeField] Image[] inventorySlotImage = new Image[9];
    [SerializeField] Image[] inventoryBackgroundImage = new Image[9];
    [SerializeField] Sprite emptySlotSprite;
    [SerializeField] Color32 slotBackgroundColor;
    [SerializeField] Camera cam;
    [SerializeField] GameObject inventory;

    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };
    private Dictionary<itemType, GameObject> itemInstantiate = new Dictionary<itemType, GameObject>() { };

    public GameObject selectedItemGameobject;


    void Start() {
        

        itemSetActive.Add(itemType.Pickaxe, pickaxe_item);
        itemSetActive.Add(itemType.Sword, sword_item);
        itemSetActive.Add(itemType.Bow, bow_item);   


        itemInstantiate.Add(itemType.Pickaxe, pickaxe_prefab);
        itemInstantiate.Add(itemType.Sword, sword_prefab);
        itemInstantiate.Add(itemType.Bow, bow_prefab);   

        NewItemSelected();
        SetSlotColors(slotBackgroundColor);
    }

    async void Update() {
     

        //Throw item

        if(Input.GetKeyDown(throwItemKey) && playerInventory.Count > 0) {
            GameObject droppedItem = Instantiate(itemInstantiate[playerInventory[selectedItem]], position: transform.position, new Quaternion());
            Debug.Log("Removed item " + selectedItem);
           
            ItemPickable itemPickable = droppedItem.GetComponent<ItemPickable>();
            if (itemPickable != null) {
                itemPickable.DropItem();
            }
            playerInventory.Remove(selectedItem);

            if(selectedItem != 0) {
                selectedItem -= 1;
            }
            NewItemSelected();
        }
        if(Input.GetKeyDown(openInventoryKey)) {
            if(inventory.activeSelf) {
                inventory.SetActive(false);
            } else {
                inventory.SetActive(true);
            }
        }

        //UI
        for (int i = 0; i < 9; ++i) {
            if (playerInventory.ContainsKey(i)) {
                inventorySlotImage[i].sprite = itemSetActive[playerInventory[i]].GetComponent<ItemPickable>().itemScriptableObject.item_sprite;
            } else {
                inventorySlotImage[i].sprite = emptySlotSprite;
            }
        }
        
        
        

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedItem = 0;
            SetSlotColors(slotBackgroundColor);
            NewItemSelected();
            if(playerInventory.ContainsKey(selectedItem)) {
                inventoryBackgroundImage[selectedItem].color = new Color32(198, 255, 158, 255);
            }
        }
         if (Input.GetKeyDown(KeyCode.Alpha2)) {
            selectedItem = 1;
            SetSlotColors(slotBackgroundColor);
            NewItemSelected();
            if(playerInventory.ContainsKey(selectedItem)) {
                inventoryBackgroundImage[selectedItem].color = new Color32(198, 255, 158, 255);
            }
        }
         if (Input.GetKeyDown(KeyCode.Alpha3)) {
            selectedItem = 2;
            SetSlotColors(slotBackgroundColor);
            NewItemSelected();
            if(playerInventory.ContainsKey(selectedItem)) {
                inventoryBackgroundImage[selectedItem].color = new Color32(198, 255, 158, 255);
            }
        }
         if (Input.GetKeyDown(KeyCode.Alpha4)) {
            selectedItem = 3;
            SetSlotColors(slotBackgroundColor);
            NewItemSelected();
            if(playerInventory.ContainsKey(selectedItem)) {
                inventoryBackgroundImage[selectedItem].color = new Color32(198, 255, 158, 255);
            }
        }
         if (Input.GetKeyDown(KeyCode.Alpha5)) {
            selectedItem = 4;
            SetSlotColors(slotBackgroundColor);
            NewItemSelected();
            if(playerInventory.ContainsKey(selectedItem)) {
                inventoryBackgroundImage[selectedItem].color = new Color32(198, 255, 158, 255);
            }

        }
         if (Input.GetKeyDown(KeyCode.Alpha6)) {
            selectedItem = 5;
            SetSlotColors(slotBackgroundColor);
            NewItemSelected();if(playerInventory.ContainsKey(selectedItem)) {
                inventoryBackgroundImage[selectedItem].color = new Color32(198, 255, 158, 255);
            }
        }
         if (Input.GetKeyDown(KeyCode.Alpha7)) {
            selectedItem = 6;
            SetSlotColors(slotBackgroundColor);
            NewItemSelected();
            if(playerInventory.ContainsKey(selectedItem)) {
                inventoryBackgroundImage[selectedItem].color = new Color32(198, 255, 158, 255);
            }
        }
         if (Input.GetKeyDown(KeyCode.Alpha8)) {
            selectedItem = 7;
            SetSlotColors(slotBackgroundColor);
            NewItemSelected();
            if(playerInventory.ContainsKey(selectedItem)) {
                inventoryBackgroundImage[selectedItem].color = new Color32(198, 255, 158, 255);
            }
        }
         if (Input.GetKeyDown(KeyCode.Alpha9)) {
            selectedItem = 8;
            SetSlotColors(slotBackgroundColor);
            NewItemSelected();
            if(playerInventory.ContainsKey(selectedItem)) {
                inventoryBackgroundImage[selectedItem].color = new Color32(198, 255, 158, 255);
            }
        }
    }
    
    private void NewItemSelected() {
        pickaxe_item.SetActive(false);
        sword_item.SetActive(false);
        bow_item.SetActive(false);
        if(playerInventory.ContainsKey(selectedItem)) {
            selectedItemGameobject = itemSetActive[playerInventory[selectedItem]];
            selectedItemGameobject.SetActive(true);
        }
       
    }
//
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Pickupable")) {
            
           
            Debug.Log("Time Difference:" + (Time.time - other.GetComponent<ItemPickable>().itemScriptableObject.lastTimeDropped).ToString());

            IPickable item = other.GetComponent<ItemPickable>();
            if(item != null) {
                if(Time.time - other.GetComponent<ItemPickable>().itemScriptableObject.lastTimeDropped > other.GetComponent<ItemPickable>().itemScriptableObject.pickupCooldown) {
                    int slot = GetNextAvailableSlot();
                    playerInventory.Add(slot, other.GetComponent<ItemPickable>().itemScriptableObject.item_type);
                    Debug.Log("ITEM PICKED UP: " + other.GetComponent<ItemPickable>().itemScriptableObject.item_type.ToString());
                    item.PickItem();
                } else {
                //other.GetComponent<ItemPickable>().itemScriptableObject.lastTimeDropped = Time.time;
            }
            }
           
            
        }
    }

    private int GetNextAvailableSlot() {
        for (int i = 0; i < 9; ++i) {
            if (!playerInventory.ContainsKey(i)) {
                return i;
            }
        }
        return -1;
    }

    private void SetSlotColors(Color32 color) {

        foreach(Image image in inventoryBackgroundImage) {
            
            image.color = color;
            
        }
    }

    

}

public interface IPickable {
    void PickItem();
}
