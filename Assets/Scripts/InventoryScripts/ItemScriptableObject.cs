using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemScriptableObject : ScriptableObject
{
    [Header("Gameplay Related")]
    public float cooldown;
    public itemType item_type;
    public actionType action_type;
    public TileBase tile;
    public Vector2Int range = new Vector2Int(5,5);
    public float pickupCooldown;
    public float lastTimeDropped;
    public bool swingable;
    public float damage;

    public float swingSpeed = 1f;
    public float swingRadius = 90f;

    public GameObject item_prefab;


    [Header("UI Related")]
    public bool stackable = true;


    [Header("UI + Gameplay Related")]
    public Sprite item_sprite;


    public void OnEnable() {
        Debug.Log("Enabling item with LTD = " + Time.time.ToString());
        lastTimeDropped = Time.time;
    }

    void Start() {
        lastTimeDropped = 0;
    }

}



public enum itemType {
  
    Pickaxe,
    Sword,
    Bow,
    Axe,
    Placeable,
    Material,
    Hammer
};

public enum actionType {
    NONE,
    Mine,
    MeleeAttack,
    ProjectileAttack,
    Chop, 
    Hammer

    
}
