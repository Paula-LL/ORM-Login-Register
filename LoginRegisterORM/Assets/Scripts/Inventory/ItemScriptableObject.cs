using UnityEngine;

[CreateAssetMenu(fileName ="Item")]

public class ItemScriptableObject : ScriptableObject
{    
    public int itemID; 
    public string itemName; 
    [TextArea]
    public string itemDesc;
    public Sprite itemImg;
    public bool isGold;


    //public GameObject lootPrefab;

    public int stackSize = 99; 
}
