using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Shop Item", menuName="ShopItem")]
public class ShopItem : ScriptableObject
{
    public new string name;
    public string displayName;
    public string description;
    public Sprite icon;
    public int cost;
}