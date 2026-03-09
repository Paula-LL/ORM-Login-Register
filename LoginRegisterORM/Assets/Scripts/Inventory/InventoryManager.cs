using System.Collections;
using System.Collections.Generic;
using TMPro; 
using UnityEngine;
public class InventoryManager : MonoBehaviour
{
    public InventorySlotManager[] itemSlots; 
    public int goldQuantity;
    public TMP_Text goldTextQuantity;
    public GameObject lootPrefab;
    public Transform player; 

    private void Start()
    {
        foreach (var slot in itemSlots) { 
            slot.UpdateInventoryUI();
        }
    }

    private void OnEnable()
    {
        Loot.OnItemLooted += AddItem; 
    }

    private void OnDisable()
    {
        Loot.OnItemLooted -= AddItem;
    }

    public void AddItem(ItemScriptableObject itemScriptable, int quantity) {// Check if the object is gold
        if (itemScriptable.isGold)
        {
            goldQuantity += quantity;
            goldTextQuantity.text = goldQuantity.ToString();
            return;
        }

        foreach (var slot in itemSlots) { //chech if you can stack objects in a slot 
            if (slot.item == itemScriptable && slot.quantity < itemScriptable.stackSize) { 
                int availableStackSpace = itemScriptable.stackSize - slot.quantity;
                int amountAdded = Mathf.Min(availableStackSpace, quantity);
                
                slot.quantity += amountAdded;
                quantity -= amountAdded;

                slot.UpdateInventoryUI();

                if (quantity <= 0) { 
                    return;
                }
            }
        }

        foreach (var slot in itemSlots) //Check for empty slots 
        {
            if (slot.item == null)
            {
                int amountToAdd = Mathf.Min(itemScriptable.stackSize, quantity);
                slot.item = itemScriptable;
                slot.quantity = amountToAdd;
                slot.UpdateInventoryUI();
                quantity -= amountToAdd;

                if (quantity <= 0)
                    return;
            }
        }

        if (quantity > 0) {
            DropItem(itemScriptable, quantity); 
        }
    }

    private void DropItem(ItemScriptableObject itemScriptable, int quantity) {
        Loot loot = Instantiate(lootPrefab, player.position, Quaternion.identity).GetComponent<Loot>();
        loot.Initialize(itemScriptable, quantity);
    }

    public void DropItemFromSlot(InventorySlotManager slot) {
        DropItem(slot.item, 1);
        slot.quantity--;
        if (slot.quantity <= 0) { 
            slot.item = null;
        }
        slot.UpdateInventoryUI(); 
    }
}
