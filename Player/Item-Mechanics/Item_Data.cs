using UnityEngine;




[CreateAssetMenu(fileName = "data", menuName = "Items", order = 0)]
public class Item_Data : ScriptableObject
{
    public enum Item_Type
    {
        BOMB,
        LIFE,
        KEY,
    }
    public Sprite icon;

    public Item_Type type;
    public string item_name;


}