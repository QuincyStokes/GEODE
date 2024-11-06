using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemScriptableObject currentItem;
    public static ItemManager instance;
    [SerializeField] private SpriteRenderer swingableSR;

    public delegate void ItemChangedHandler(ItemScriptableObject newItem);
    public event ItemChangedHandler OnItemChanged;

    void Awake() 
    {
        if (instance == null) 
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateSwingableSprite();
    }

    public ItemScriptableObject GetCurrentItem()
    {
        return currentItem;
    }

    public void SetCurrentItem(ItemScriptableObject newItem)
    {
        if (currentItem != newItem)
        {
            currentItem = newItem;
            UpdateSwingableSprite();
            OnItemChanged?.Invoke(newItem);
        }
    }

    private void UpdateSwingableSprite()
    {
        if (swingableSR != null && currentItem != null)
        {
            swingableSR.sprite = currentItem.item_sprite;
            Debug.Log("Changing item to " + currentItem.item_type);
        }
    }
}
