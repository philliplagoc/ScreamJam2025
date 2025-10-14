using System;
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

    public void AddItem(Item item)
    {
        if (Items.ContainsKey(item.Type))
        {
            Items[item.Type] += item.Value;
        }
        else
        {
            Items.Add(item.Type, item.Value);
        }
        
        // Update UI
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.UpdateUI();
        }
        
        CheckIfAllGunPartsAreCollected();
    }

    /// <summary>
    /// This is the win condition of the game
    /// </summary>
    private void CheckIfAllGunPartsAreCollected()
    {
        int gunPartsCount = 0;
        foreach (var item in Items.Keys)
        {
            if (item == ItemType.GunPart)
            {
                gunPartsCount++;
            }
        }

        if (gunPartsCount >= 5)
        {
            Debug.Log("All gun parts collected, you win!");
        }
    }
}
