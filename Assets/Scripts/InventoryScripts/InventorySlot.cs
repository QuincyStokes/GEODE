using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Sprite selectedImage, unselectedImage;


    public void Awake() {
        Deselect();
    }
    public void Select() {
        image.sprite = selectedImage;

    }

    public void Deselect() {
        image.sprite = unselectedImage;
    }

    public void OnDrop(PointerEventData eventData) {
        if(transform.childCount == 0) {
            //This gets our dropped item from whatever we dragged in eventData
            GameObject dropped = eventData.pointerDrag;
            //Our dropped item's component called "DraggableItem" gets stored in draggableItem. This is the InventoryItem script
            InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();
            //We access the grabbed InventoryItem script, and access its parentAfterDrag variable. We then set it to THIS object's transform. 
            draggableItem.parentAfterDrag = transform;
        }
       
    }
}
