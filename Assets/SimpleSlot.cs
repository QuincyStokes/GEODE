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

   
}
