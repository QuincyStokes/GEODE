using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftButton : MonoBehaviour
{
   public CraftingRecipe craftingRecipe;

   public void OnCraft() {
        //check if inventory has all required items
            //if it does, craft that jon (take away items from inventory, add result item)
        // if it doesn't just for now print no bitch

        //loop through items in Materials
            //if the item is in the inventory, 
                //if we have enough of that item
        bool canCraft = true;
        foreach (ItemAmount material in craftingRecipe.Materials) {
            if(InventoryManager.instance.ContainsItem(material.Item)) { //if the item is in the inventory
                print(InventoryManager.instance.ItemCount(material.Item));
                print(material.Amount);
                if(InventoryManager.instance.ItemCount(material.Item) < material.Amount) { //if we have enough of the item
                    print("not enough materials");
                    canCraft = false;
                }
            }
            
            if(!InventoryManager.instance.ContainsItem(material.Item)) {
                canCraft = false;
            }
            
        }
        //if we get here, that means we can craft it
        if(canCraft == true) {
            foreach (ItemAmount material in craftingRecipe.Materials) {
                print("Removing" + material.Item.item_type);
                InventoryManager.instance.RemoveItem(material.Item, material.Amount);
            }
            print("Adding item");
            InventoryManager.instance.AddItem(craftingRecipe.Results[0].Item);
            AudioManager.instance.Play("craft");
        
        }
        //this removes all the items from their inventory
        

   }
}
