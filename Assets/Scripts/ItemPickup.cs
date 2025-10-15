using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPickup : MonoBehaviour, IPointerClickHandler
{
    public Item ItemToPickup;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {ItemToPickup.Name}");
        
        InventoryManager.Instance.AddItem(ItemToPickup.Type, ItemToPickup.Value);

        Destroy(gameObject);
    }
}
