using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Text countText;

    public Item item;
    public int count = 1;
    public Transform parentAfterDrag;
    public Transform parentBeforeDrag;

    public Slider waterSlider;

    public void Update()
    {
        if (item != null && item.actionType == ActionType.Water)
        {
            waterSlider.value = item.currentWater;
            waterSlider.maxValue = item.waterAmount;
        }

        //if(transform.parent != null && transform.parent.GetComponent<InventorySlot>() != null)
        //{
        //    if (transform.parent.GetComponent<InventorySlot>().seedSlot)
        //    {
        //        transform.GetComponent<Image>().raycastTarget = false;
        //    }
        //}        
    }

    public void InitialiseItem(Item newItem)
    {
        if(newItem.actionType == ActionType.Water)
        {
            waterSlider.gameObject.SetActive(true);
            waterSlider.value = newItem.currentWater;
            waterSlider.maxValue = newItem.waterAmount;
        }
        else
        {
            waterSlider.gameObject.SetActive(false);
        }
        item = newItem;
        image.sprite = newItem.image;
        parentBeforeDrag = transform.parent;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    // Drag and Drop
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item.type != ItemType.Seed)
        {
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
        }
        else
        {
            image.raycastTarget = false;
            transform.SetParent(parentBeforeDrag);
            transform.position = parentBeforeDrag.position;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (item.type != ItemType.Seed)
            transform.position = Input.mousePosition;
        else
        {
            transform.SetParent(parentBeforeDrag);
            transform.position = parentBeforeDrag.position;
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (item.type != ItemType.Seed)
        {
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
        }
        else
        {
            image.raycastTarget = true;
            transform.SetParent(parentBeforeDrag);
            transform.position = parentBeforeDrag.position;
        }
    }
}
