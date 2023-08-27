using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;

    private ToolBarNumber toolBarNumber;
    public InventoryManager inventoryManager;

    public bool brewingSlot = false;
    public bool seedSlot = false;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }

    // Drag and Drop
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (brewingSlot)
        {
            if (inventoryItem.item.type == ItemType.HarvestProd)
            {
                inventoryItem.parentAfterDrag = transform;
            }
        }
        else if (!brewingSlot && transform.childCount == 0)
        {
            inventoryItem.parentAfterDrag = transform;
        }
    }

    //Item Selection
    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.parent.gameObject.CompareTag("Toolbar"))
        {
            toolBarNumber = GetComponent<ToolBarNumber>();
            inventoryManager.ChangeSelectedSlot(toolBarNumber.number);
        }
    }
}
