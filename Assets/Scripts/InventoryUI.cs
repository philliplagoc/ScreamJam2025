using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public GameObject InventorySlotPrefab;  // Prefab for the slot
    public Transform ItemsParent;           // Panel to put slots in

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

    /// <summary>
    /// Called when entire UI is refreshed.
    /// </summary>
    public void UpdateUI()
    {
        // Clear all existing slots to avoid duplicates
        foreach (Transform child in ItemsParent)
        {
            Destroy(child.gameObject);
        }
        
        // Loop through inventory and create slot for each item
        Dictionary<Item, int> items = InventoryManager.Instance.Items;
        foreach (var itemEntry in items)
        {
            GameObject slotGO = Instantiate(InventorySlotPrefab, ItemsParent);
            
            // Get components of the slot
            Image itemIcon = slotGO.transform.Find("ItemIcon").GetComponent<Image>();
            TextMeshProUGUI quantityText = slotGO.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
            
            // Set icon
            itemIcon.sprite = itemEntry.Key.Icon;
            itemIcon.enabled = true;
            
            // Set quantity text if greater than 1
            if (itemEntry.Value > 1 && itemEntry.Key.IsStackable)
            {
                quantityText.text = itemEntry.Value.ToString();
            }
            else
            {
                quantityText.text = "";
            }
        }
    }
}
