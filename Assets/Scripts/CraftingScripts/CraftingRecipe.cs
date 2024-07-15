
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemAmount {
    
    public ItemScriptableObject Item; //For any recipe, this is an item required
    [Range(1, 999)]
    public int Amount; //For any item required in a recipe, this is the amount

    
}


[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject {
    public List<ItemAmount> Materials;
    public List<ItemAmount> Results;

    public bool CanCraft(IItemContainer itemContainer) {
        foreach (ItemAmount itemAmount in Materials) {
            if(itemContainer.ItemCount(itemAmount.Item) < itemAmount.Amount) {
                return false;
            }
        }
        return true;
    }

    public void Craft(IItemContainer itemContainer) {
        if(CanCraft(itemContainer)) {
            foreach (ItemAmount itemAmount in Materials) {
                for(int i = 0; i < itemAmount.Amount; ++i) {
                    itemContainer.RemoveItem(itemAmount.Item, itemAmount.Amount);
                }
                
            }
            foreach (ItemAmount itemAmount in Results) {
                for(int i = 0; i < itemAmount.Amount; ++i) {
                    itemContainer.AddItem(itemAmount.Item);
                }
                
            }
        }
    }
   
}
