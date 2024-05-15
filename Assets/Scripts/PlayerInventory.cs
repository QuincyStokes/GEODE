using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("General")]

    public List<itemType> inventoryList;
    public int selectedItem;
    public float playerReach;

    [Space(20)]
    [Header("Keys")]
    [SerializeField] KeyCode throwItemKey;
    [SerializeField] KeyCode pickItemKey;

    [Space(20)]
    [Header("Item gameobjects")]
    [SerializeField] GameObject pickaxe_item;
    [SerializeField] GameObject sword_item;
    [SerializeField] GameObject bow_item;

    [SerializeField] Camera cam;

    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };

    void Start() {
        itemSetActive.Add(itemType.Pickaxe, pickaxe_item);
        itemSetActive.Add(itemType.Sword, sword_item);
        itemSetActive.Add(itemType.Bow, bow_item);    

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
//asd
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Pickupable")) {
            Debug.Log("ITEM PICKED UP");
            IPickable item = other.GetComponent<ItemPickable>();
            inventoryList.Add(other.GetComponent<Collider>().itemScriptableObject.item_type);
            item.PickItem();
        }
    }

}

public interface IPickable {
    void PickItem();
}
