using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
    public List<GameObject> contentMenus;
    public void ChangeCraftingTab(GameObject menu) {
        foreach (GameObject go in contentMenus) {
            go.SetActive(false);
        }
        menu.SetActive(true);
    }
}
