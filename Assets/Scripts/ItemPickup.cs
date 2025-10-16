using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPickup : MonoBehaviour, IPointerClickHandler
{
    public Item ItemToPickup;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {ItemToPickup.Name}");
        
        // Play sound fx
        if (ItemToPickup.Type == ItemType.Wood)
        {
            // Play wood collecting fx
            if (AudioManager.Instance != null) 
                AudioManager.Instance.PlaySFX(AudioManager.Instance.CollectWoodSoundFx);
        } else if (ItemToPickup.Type == ItemType.GunPart)
        {
            // Play gun sound fx
            if (AudioManager.Instance != null) 
                AudioManager.Instance.PlaySFX(AudioManager.Instance.CollectGunPartSoundFx);
        }
        
        InventoryManager.Instance.AddItem(ItemToPickup.Type, ItemToPickup.Value);

        Destroy(gameObject);
    }
}
