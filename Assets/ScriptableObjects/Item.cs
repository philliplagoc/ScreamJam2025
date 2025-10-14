using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string ItemName = "New Item";
    public Sprite Icon = null;
    public bool IsStackable = true;
    public int Value = 0;
    public bool IsGunPart = false;
}
