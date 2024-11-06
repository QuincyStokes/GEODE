using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSlot : MonoBehaviour
{
    [SerializeField] public Image image;
    [SerializeField] public TMP_Text amountText; 
    [SerializeField] public TMP_Text descriptionText;
    
    public List<GameObject> materialSlots;
    public GameObject resultSlot;
    public CraftingRecipe craftingRecipe;
    public GameObject craftButton;



    //on click thing
        //set the materialSlots' attributes to this crafting recipe's attributes
    void Start() {
        Initialize();
    }

    void Initialize() {
        image.sprite = craftingRecipe.Results[0].Item.item_sprite;
        image.preserveAspect = true;
        amountText.text = craftingRecipe.Results[0].Amount.ToString();
        resultSlot.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
    }

    public void OnSlotClicked() {
        //set our material slots
        //set the result slot
        print("crafting slot clicked");

        //for every crafting material, go through and set the materialslot[index] to that iamge nad amount
        //when were done, set any remaining crafting slots to inactive
        int index = 0;
        foreach (ItemAmount material in craftingRecipe.Materials) {
            materialSlots[index].GetComponent<SimpleSlot>().image.sprite = material.Item.item_sprite;
            materialSlots[index].GetComponent<SimpleSlot>().amountText.text = material.Amount.ToString();
            descriptionText.text = craftingRecipe.Results[0].Item.description;

            index++;
            materialSlots[index].gameObject.SetActive(true);
        }

        for (int i = index; i < materialSlots.Count; ++i) {
            materialSlots[i].gameObject.SetActive(false);
        }

        //good
        craftButton.GetComponent<CraftButton>().craftingRecipe = craftingRecipe;
        resultSlot.transform.GetChild(0).GetComponent<Image>().sprite = craftingRecipe.Results[0].Item.item_sprite;
    }
}
