using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tile/Custom Rule Tile")]
public class RuleTileWithData : RuleTile
{
    public ItemScriptableObject item;
    public ItemScriptableObject entity;
    public actionType action_type;
    public int health;
}
