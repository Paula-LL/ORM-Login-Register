using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public ItemScriptableObject item;
    public TMP_Text itemName;
    public TMP_Text priceItem;
    public Image itemImg;

    public int itemPrice;



    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private ShopInformation shopInfo; 

    private void Start()
    {
;
    }

    public void Initialize(ItemScriptableObject itemScriptable, int itemPrice)
    {   
        item = itemScriptable;
        itemImg.sprite = item.itemImg;
        itemName.text = item.itemName;
        this.itemPrice = itemPrice; 
        priceItem.text = itemPrice.ToString();
    }

    public void OnBuyButtonClicked()
    {
        shopManager.TryBuyingItem(item, itemPrice); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null) { 
            shopInfo.ShowItemInfo(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shopInfo.HideItemInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (item != null) {
            shopInfo.FollowMouse(); 
        }
    }
}
