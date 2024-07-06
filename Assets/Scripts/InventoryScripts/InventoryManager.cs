using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance;

    public ItemScriptableObject[] startItems;

    public int maxStackedItems = 10;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public GameObject inventory;

    int selectedSlot = -1; //default -1 because nothing selected

    private void Awake() {
        instance = this;
    }

    void Start() {
        ChangeSelectedSlot(0);
        foreach(var item in startItems) {
            AddItem(item);
        }
    }

    private void Update() {
        if (Input.inputString != null) {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 10) {
                ChangeSelectedSlot(number-1);
            }
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            if(inventory.activeSelf == true) {
                inventory.SetActive(false);
            }  else {
                inventory.SetActive(true);
            } 
        }

        if(Input.mouseScrollDelta.y > 0) {
            ChangeSelectedSlot(selectedSlot-1);
        } else if (Input.mouseScrollDelta.y < 0) {
            ChangeSelectedSlot(selectedSlot+1);
        }
    }

    void ChangeSelectedSlot(int newValue) {
        if(selectedSlot >= 0) {
            inventorySlots[selectedSlot].Deselect();
        }
        if(newValue > 8) {
            inventorySlots[0].Select();
            selectedSlot = 0;
        } else if (newValue < 0) {
            inventorySlots[8].Select();
            selectedSlot = 8;
        } else {
            inventorySlots[newValue].Select();
            selectedSlot = newValue;
        }
       
    }

    public bool AddItem(ItemScriptableObject item) {

        for (int i = 0; i < inventorySlots.Length; i++) {
            //current inventory slot -> get the component called "InventoryItem" of its CHILD
            //if its null, that means there is no item in this slot.
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.stackable) {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }


        //First find an empty slot
        for (int i = 0; i < inventorySlots.Length; i++) {
            //current inventory slot -> get the component called "InventoryItem" of its CHILD
            //if its null, that means there is no item in this slot.
            if (inventorySlots[i].GetComponentInChildren<InventoryItem>() == null) {
                CreateItem(item, inventorySlots[i]);
                return true;
            }
        }
        return false;
    }

    void CreateItem(ItemScriptableObject item, InventorySlot slot) {
        //Create a new gameObject, instantiate it with our Item prefab. This will be the item in our hotbar.
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        //Create a new InventoryItem object (the script for inventoryItem), grab it from the item we just created.
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        //Initialize the item passed in to the inventoryItem script we just grabbed.
        //This gives the NEW inventoryItem the data of the item passed in.
        inventoryItem.InitializeItem(item);
    }

    public ItemScriptableObject GetSelectedItem(bool use) {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if(itemInSlot != null) {
            ItemScriptableObject item = itemInSlot.item;
            if(use == true) {
                itemInSlot.count--;
                if(itemInSlot.count <= 0) {
                    Destroy(itemInSlot.gameObject);
                } else {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        } 
        return null;
    }
}
