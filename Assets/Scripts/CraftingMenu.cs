using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraftingMenu : MonoBehaviour
{
    public List<GameObject> contentMenus;
    public List<GameObject> craftingTabs;
    public List<GameObject> craftingTabBlends;
    [SerializeField] private Sprite deactivatedImage;
    [SerializeField] private Sprite activatedImage;
    public void ChangeCraftingTab(int num) {
        
        for(int i = 0; i < contentMenus.Count; ++i) {
            contentMenus[i].SetActive(false);
            craftingTabs[i].GetComponent<Image>().sprite = deactivatedImage;
            craftingTabBlends[i].SetActive(false);
            
        }
        contentMenus[num].SetActive(true);
        craftingTabBlends[num].SetActive(true);
        craftingTabs[num].GetComponent<Image>().sprite = activatedImage;
        


       
        //menu. transform.Image.sprite = activatedImage;
    }



}
