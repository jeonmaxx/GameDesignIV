using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpItem : PlayerNear, IPointerClickHandler
{
    public Item itemToPick;
    public InventoryManager inventoryManager;
    public bool pickedUp;

    public void Update()
    {
        if (pickedUp)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CalcDistance();
        if(isPlayerNear)
        {            
            inventoryManager.AddItem(itemToPick);
            pickedUp = true;
        }
    }
}
