using UnityEngine;

// Define the enum outside the ScriptableObject class for broader accessibility
public enum ItemType
{
    Wood,
    GunPart
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string Name = "New Item";
    public int Value = 0;
    public ItemType Type;
}
