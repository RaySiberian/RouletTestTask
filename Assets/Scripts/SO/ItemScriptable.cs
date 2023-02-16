using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ItemScriptable : ScriptableObject
{
    public string Name;
    public Color Color;
}
