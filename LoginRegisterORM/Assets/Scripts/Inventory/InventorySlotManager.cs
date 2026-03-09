using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotManager : MonoBehaviour
{
    public ItemScriptableObject item;
    public int quantity;
    public Image itemImage;
    public TMP_Text quantityText;

    private InventoryManager inventory;
    [SerializeField] Button slotButton;

    private static ShopManager activeShop;

    private void OnEnable()
    {
        slotButton.onClick.AddListener(ButtonPressedClick);
        ShopKeeperManager.OnShopStateChange += HandleShopStateChange;     
    }

    private void OnDisable()
    {
        slotButton.onClick.AddListener(ButtonPressedClick);
        ShopKeeperManager.OnShopStateChange -= HandleShopStateChange;
    }

    private void HandleShopStateChange(ShopManager shopManager, bool isOpen) { 
           activeShop  = isOpen ? shopManager : null;
    }


    private void Start()
    {
        inventory = GetComponentInParent<InventoryManager>();
    }

    public void ButtonPressedClick()// need to differentiate between right and left click so events dont mix.
    {
        if (quantity > 0)
        {
            if (activeShop != null)
            {
                activeShop.SellItems(item);
                quantity--;
                UpdateInventoryUI();
            }
            else {
                inventory.DropItemFromSlot(this);
            }
        }
    }

    public void UpdateInventoryUI()
    {
        if (quantity <= 0) {
            item = null; 
        }

        if (item != null)
        {
            itemImage.sprite = item.itemImg;
            itemImage.gameObject.SetActive(true);
            quantityText.text = quantity.ToString();
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            quantityText.text = "";
        }
    }

}
