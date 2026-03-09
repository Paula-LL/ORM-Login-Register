using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopInformation : MonoBehaviour
{
    public CanvasGroup infoPanel;
    public TMP_Text itemNameText; 
    public TMP_Text itemDescText;

    private RectTransform infoPanelRect;

    private void Awake()
    {
        infoPanelRect = GetComponent<RectTransform>();
    }

    public void ShowItemInfo(ItemScriptableObject itemScriptable) { 
        infoPanel.alpha = 1;
        itemNameText.text = itemScriptable.name;
        itemDescText.text = itemScriptable.itemDesc;
    }

    public void HideItemInfo() {
        infoPanel.alpha = 0;

        itemNameText.text = "";
        itemDescText.text = "";
    }

    public void FollowMouse() { 
        Vector3 mousePosition = Input.mousePosition;
        Vector3 offset = new Vector3(200, -200, 0);

        infoPanelRect.position = mousePosition + offset;
    }

}
