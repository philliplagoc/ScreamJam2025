using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Maps items (key) to quantities (value)
    // In this prototype, there are only two types of items:
    //  - Wood
    //  - GunPart
    public Dictionary<ItemType, int> Items = new Dictionary<ItemType, int>();

    public static InventoryManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemType itemType, int amount)
    {
        if (Items.ContainsKey(itemType))
        {
            Items[itemType] += amount;
        }
        else
        {
            Items.Add(itemType, amount);
        }
        
        // Update UI
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.UpdateUI();
        }
        
        // Notify GameManager to check for the win condition
        if (itemType == ItemType.GunPart && GameManager.Instance != null)
        {
            GameManager.Instance.CheckWinCondition(Items[ItemType.GunPart]);
        }
    }
    
    public bool RemoveItem(ItemType itemType, int amount)
    {
        if (Items.ContainsKey(itemType) && Items[itemType] >= amount)
        {
            Items[itemType] -= amount;

            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.UpdateUI();
            }

            return true;
        }
        
        // Not enough items to remove.
        Debug.Log($"{amount} of type {itemType} is too much.");
        return false;
    } 
}
