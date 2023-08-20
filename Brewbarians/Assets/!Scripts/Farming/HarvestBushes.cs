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

    private void Update()
    {
        if (emptyBool)
        {
            SpriteRenderer bushImage = GetComponent<SpriteRenderer>();
            bushImage.sprite = emptyImage;
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
