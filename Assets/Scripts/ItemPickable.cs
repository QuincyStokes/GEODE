using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickable : MonoBehaviour, IPickable
{
    public ItemScriptableObject itemScriptableObject;
    
    public void PickItem() {
        Destroy(gameObject);
    }

    public void DropItem() {
        itemScriptableObject.lastTimeDropped = Time.time;
    }
}
