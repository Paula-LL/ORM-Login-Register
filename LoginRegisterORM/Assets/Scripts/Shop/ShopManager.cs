using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopSlotManager[] shopSlotManager;

    [SerializeField] private InventoryManager inventoryManager; 

    public void PopulateShopItems(List<ShopItems> shopItems) {
        for (int i = 0; i < shopItems.Count && i < shopSlotManager.Length; i++) {
            ShopItems shopItem = shopItems[i];
            shopSlotManager[i].Initialize(shopItem.itemsShop, shopItem.priceItem);
            shopSlotManager[i].gameObject.SetActive(true);
        }

        for (int i = shopItems.Count; i < shopSlotManager.Length; i++)
        {
            shopSlotManager[i].gameObject.SetActive(false);
        }
    }

    public void TryBuyingItem(ItemScriptableObject itemScriptable, int price) { 
        if (itemScriptable != null && inventoryManager.goldQuantity >= price)
        {
            if (HasSpaceForItem(itemScriptable)) { 
                inventoryManager.goldQuantity -= price;
                inventoryManager.goldTextQuantity.text = inventoryManager.goldQuantity.ToString();
                inventoryManager.AddItem(itemScriptable, 1);
            }
        }
    }

    private bool HasSpaceForItem(ItemScriptableObject itemScriptable)
    {
        foreach (var slot in inventoryManager.itemSlots)
        {
            if (slot.item == itemScriptable && slot.quantity < itemScriptable.stackSize)
            {
                return true;
            }
            else if (slot.item == null)
            {
                return true;
            }
        }

        return false;
    }

    public void SellItems(ItemScriptableObject itemScriptable) {
        if (itemScriptable == null) {
            return;
        }

        foreach (var slot in shopSlotManager) {
            if (slot.item == itemScriptable) {
                inventoryManager.goldQuantity += slot.itemPrice;
                inventoryManager.goldTextQuantity.text = inventoryManager.goldQuantity.ToString();
                return; 
            }
        }
    }

}

[System.Serializable]

public class ShopItems {
    public ItemScriptableObject itemsShop;
    public int priceItem;   
}

