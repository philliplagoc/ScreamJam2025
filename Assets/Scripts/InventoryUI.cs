using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [SerializeField] private TextMeshProUGUI m_woodCollectedText;
    [SerializeField] private TextMeshProUGUI m_gunPartsCollectedText;

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
        foreach (var item in InventoryManager.Instance.Items)
        {
            // Update wood counter UI
            if (item.Key == ItemType.Wood)
            {
                m_woodCollectedText.text = $"Wood: {item.Value}";
            }
            else if (item.Key == ItemType.GunPart)
            {
                m_gunPartsCollectedText.text = $"Gun Parts: {item.Value}";
            }
        }
    }
}
