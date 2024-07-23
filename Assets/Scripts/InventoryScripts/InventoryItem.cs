using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
   //Drag and drop

    [Header("UI")]
    public Image image;
    public SpriteRenderer sr;
    public TMP_Text countText;

    [HideInInspector] public ItemScriptableObject item;
    public int count = 1;

    [HideInInspector] public Transform parentAfterDrag;
    private GameObject tooltip;
    private TMP_Text tooltipText;


    

    public void InitializeItem(ItemScriptableObject newItem) {
        item = newItem;
        image.sprite = newItem.item_sprite;
        image.preserveAspect = true;
        RefreshCount();
        GenerateTooltip();
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

    public void OnPointerEnter(PointerEventData eventData) {
        if(InventoryManager.instance.inventory.activeSelf == true) {
            tooltip.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltip.SetActive(false);
    }

    private void GenerateTooltip() {
        tooltip = transform.Find("Tooltip").gameObject;
        if(tooltip != null) {
            tooltipText = GetComponentInChildren<TMP_Text>(true);
            
            print(tooltipText.text);
            if(tooltipText != null) {
                tooltipText.text = "";
                print(tooltipText.text);
                tooltipText.text = item.item_name.ToString() + '\n' + '\n';
                if(item.damage != 0) {
                    tooltipText.text += "Damage: " + item.damage + '\n';
                }
                if(item.swingSpeed != 0) {
                    tooltipText.text += "Swing Speed: " + item.swingSpeed + '\n';
                }
                tooltipText.text += item.description;
                print(tooltipText.text);
                //will put durability here
            
            }
        } else {
            print("Tooltip not found.");
        }

    }

    
}
