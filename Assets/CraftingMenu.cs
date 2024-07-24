using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraftingMenu : MonoBehaviour
{
    public List<GameObject> contentMenus;
    public List<GameObject> craftingTabs;
    [SerializeField] private Sprite deactivatedImage;
    [SerializeField] private Sprite activatedImage;
    public void ChangeCraftingTab(int num) {
        
        for(int i = 0; i < contentMenus.Count; ++i) {
            contentMenus[i].SetActive(false);
            craftingTabs[i].GetComponent<Image>().sprite = deactivatedImage;
            craftingTabs[i].transform.localScale = new Vector3(.75f, .75f, 1f);
        }
        contentMenus[num].SetActive(true);
        craftingTabs[num].GetComponent<Image>().sprite = activatedImage;
        craftingTabs[num].transform.localScale = new Vector3(1.5f, 1.5f, 1f);


       
        //menu. transform.Image.sprite = activatedImage;
    }



}
