using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPickup : MonoBehaviour
{
    public Item ItemToPickup;

    public void OnPointerClikc(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {ItemToPickup.Name}");
        
        InventoryManager.Instance.AddItem(ItemToPickup);

        Destroy(gameObject);
    }
}
