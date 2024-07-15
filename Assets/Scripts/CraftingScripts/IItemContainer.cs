
public interface IItemContainer {
    bool ContainsItem(ItemScriptableObject item);
    int ItemCount(ItemScriptableObject item);
    bool RemoveItem(ItemScriptableObject item, int amount);
    bool AddItem(ItemScriptableObject item);
    bool IsFull();
    
}
    

