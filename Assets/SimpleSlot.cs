using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleSlot : MonoBehaviour
{
   public Image image;
   public TMP_Text amountText;
   [Range(1, 999)]
   public int amount;
   /*
   void Start() {
      if(transform.childCount > 0) {
         Transform child = transform.GetChild(0);
         child.gameObject.SetActive(false);
         //child.GetComponent<Image>().color = new Color(255, 255, 255, 0);
      }
      if (transform.childCount > 1) {
         Transform textChild = transform.GetChild(1);
         textChild.gameObject.SetActive(false);
      }
   }
   */
}
