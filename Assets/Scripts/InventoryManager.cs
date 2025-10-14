using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Maps items (key) to quantities (value)
    public Dictionary<Item, int> Items = new Dictionary<Item, int>();

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
        if (Items.ContainsKey(item))
        {
            if (item.IsStackable)
            {
                Items[item] += item.Value;
            }
        }
        else
        {
            Items.Add(item, item.Value);
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
            if (item.IsGunPart)
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
