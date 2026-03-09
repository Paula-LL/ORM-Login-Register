using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Loot : MonoBehaviour
{
    public ItemScriptableObject item;
    public SpriteRenderer spriteRend;
    public Animator animator;

    public bool canPickUp = true; 
    public int quantity;
    bool pickedUp = false;

    public static event Action<ItemScriptableObject, int> OnItemLooted; 

    private void Start()
    {
        if (item == null)
        {
            return;
        }
        UpdateImg();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickedUp) return;
        if (collision.CompareTag("Player") && canPickUp == true)
        {
            animator.Play("LootPickUp");
            pickedUp = true;
            OnItemLooted?.Invoke(item, quantity); 
            Destroy(gameObject, 5f);
        }
    }

    public void Initialize(ItemScriptableObject itemScriptable, int quantity) { 
        item = itemScriptable;
        this.quantity = quantity;
        canPickUp = false; 
        UpdateImg();

    }

    private void UpdateImg() {
        spriteRend.sprite = item.itemImg;
        this.name = item.itemName;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            canPickUp = true; 
        }
    }
}
