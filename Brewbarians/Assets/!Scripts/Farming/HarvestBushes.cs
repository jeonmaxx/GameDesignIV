using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HarvestBushes : PlayerNear, IPointerDownHandler
{
    public Sprite emptyImage;
    public Item harvestItem;
    public bool emptyBool;
    public InventoryManager inventoryManager;
    private MouseOnInteractable onInteractable;

    private void Start()
    {
        onInteractable = GetComponent<MouseOnInteractable>();
    }

    private void Update()
    {
        if (emptyBool)
        {
            SpriteRenderer bushImage = GetComponent<SpriteRenderer>();
            bushImage.sprite = emptyImage;
            onInteractable.interactable = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CalcDistance();
        if(isPlayerNear && !emptyBool)
        {
            inventoryManager.AddItem(harvestItem);
            emptyBool = true;
        }
    }
}
