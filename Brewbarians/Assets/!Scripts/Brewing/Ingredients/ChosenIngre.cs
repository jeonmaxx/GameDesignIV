using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Product { One, Two }
public class ChosenIngre : MonoBehaviour
{
    public Item chosenItem;
    private InventoryItem invenItem;
    public GameObject brewingStation;
    private BrewingManager brewingManager;
    public Product product;

    public void Start()
    {
        brewingManager = brewingStation.GetComponent<BrewingManager>();
    }

    public void Update()
    {
        CheckForItem();
    }

    public void CheckForItem()
    {
        if(transform.childCount != 0)
        {
            invenItem = GetComponentInChildren<InventoryItem>();
            chosenItem = invenItem.item;
        }
        else if(transform.childCount == 0)
        {
            chosenItem = null;
        }

        if(product == Product.One)
        {
            brewingManager.itemOne = chosenItem;
        }
        else if(product == Product.Two)
        {
            brewingManager.itemTwo = chosenItem;
        }
    }
}
