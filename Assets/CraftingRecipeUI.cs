using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeUI : MonoBehaviour
{
   [Header("References")]

   [Header("Public Variables")]
   public CraftingRecipe craftingRecipe;
   


   private void Start() {
 
        //SetCraftingRecipe(craftingRecipe);
   }
   

/*
    private void SetCraftingRecipe(CraftingRecipe newCraftingRecipe) {
        craftingRecipe = newCraftingRecipe;
        if (craftingRecipe != null) {
            int slotIndex = 0;
            //set the first x amount of slots to crafting materials
            slotIndex = SetSlots(craftingRecipe.Materials, slotIndex);
            //set the arrow to the next available slot
            //arrowParent.SetSiblingIndex(slotIndex);
            //set the remaining required slots as the result 
            slotIndex = SetSlots(craftingRecipe.Results, slotIndex);

            //any remaining slots will be turned off.
            for (int i = slotIndex; i < craftingSlots.Length; ++i) {
                craftingSlots[i].transform.gameObject.SetActive(false);
            }
            //enable the entire crafting recipe UI
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }
*/ 
/*
    private int SetSlots(IList<ItemAmount> itemAmountList, int slotIndex) {
        for (int i = 0; i < itemAmountList.Count; ++i, ++slotIndex) {
            ItemAmount itemAmount = itemAmountList[i];
            CraftingSlot craftSlot = craftingSlots[slotIndex];
        
            craftSlot.item = itemAmount.Item;
            craftSlot.amount = itemAmount.Amount;
            craftSlot.transform.parent.gameObject.SetActive(true);
            craftSlot.refresh();
            
        }
        return slotIndex;
    }
    */
}
