using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeperManager : MonoBehaviour
{
    public Animator anim;
    public CanvasGroup shopCanvasGroup;
    public ShopManager shopManager;

    private bool playerInRange;
    private bool isShopOpen;

    public static event Action<ShopManager, bool> OnShopStateChange;

    [SerializeField] private List<ShopItems> shopItems;
    [SerializeField] private List<ShopItems> shopWeapons;
    [SerializeField] private List<ShopItems> shopArmour;


    void Update() {
        if (playerInRange)
        {
            if (Input.GetButtonDown("Interact")) {
                if (!isShopOpen) {
                    Time.timeScale = 0;
                    isShopOpen = true;
                    OnShopStateChange?.Invoke(shopManager, true);
                    shopCanvasGroup.alpha = 1;
                    shopCanvasGroup.blocksRaycasts = true;
                    shopCanvasGroup.interactable = true;
                    OpenItemShop(); 
                } else
                {
                    Time.timeScale = 1;
                    isShopOpen = false;
                    OnShopStateChange?.Invoke(shopManager, false);
                    shopCanvasGroup.alpha = 0;
                    shopCanvasGroup.blocksRaycasts = false;
                    shopCanvasGroup.interactable = false;
                }
            }
        }
    }

    public void OpenItemShop() { 
        shopManager.PopulateShopItems(shopItems);
    }

    public void OpenWeaponShop() {
        shopManager.PopulateShopItems(shopWeapons);
    }

    public void OpenArmourShop() {
        shopManager.PopulateShopItems(shopArmour);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            anim.SetBool("PlayerInRange", true); 
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("PlayerInRange", false);
            playerInRange = false;
        }
    }
}
