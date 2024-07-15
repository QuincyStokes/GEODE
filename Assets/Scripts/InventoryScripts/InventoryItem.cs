using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
   //Drag and drop

    [Header("UI")]
    public Image image;
    public SpriteRenderer sr;
    public TMP_Text countText;

    [HideInInspector] public ItemScriptableObject item;
    public int count = 1;

    [HideInInspector] public Transform parentAfterDrag;

    

    public void InitializeItem(ItemScriptableObject newItem) {
        item = newItem;
        image.sprite = newItem.item_sprite;
        image.preserveAspect = true;
        RefreshCount();
    }

    public void RefreshCount() {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
        if(count == 0) {
           Destroy(gameObject);
        }
    }


    public void OnBeginDrag(PointerEventData eventData) {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        countText.raycastTarget = false;
    } 

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        countText.raycastTarget = true;
    }
    
}
