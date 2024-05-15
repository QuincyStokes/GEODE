using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemScriptableObject : ScriptableObject
{
    [Header("Properties")]
    public float cooldown;
    public itemType item_type;
    public Sprite item_sprite;
   
    public float pickupCooldown;
    public float lastTimeDropped;

    public void OnEnable() {
        Debug.Log("Enabling item with LTD = " + Time.time.ToString());
        lastTimeDropped = Time.time;
    }

}



public enum itemType {
    Pickaxe,
    Sword,
    Bow
};
