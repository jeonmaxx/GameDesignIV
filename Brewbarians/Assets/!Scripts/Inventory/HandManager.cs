using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item handItem;
    public int count;

    public void Update()
    {
        GetHandItem();
    }

    public void GetHandItem()
    {
        Item selectedItem = inventoryManager.GetSelectedItem(false);
        if (selectedItem != null)
        {
            handItem = selectedItem;
            count = inventoryManager.itemInSlot.count;            
        }
        else
        {
            handItem = null;
            count = 0;
        }
    }
}
