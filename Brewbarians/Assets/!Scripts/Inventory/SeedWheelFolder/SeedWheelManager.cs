using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedWheelManager : MonoBehaviour
{
    public Item chosenSeed;
    public Text text;
    public GameObject seedObj;

    private InventoryItem inventoryItem;

    public void Update()
    {
        if(seedObj != null)
        {
            inventoryItem = seedObj.GetComponent<InventoryItem>();
            chosenSeed = inventoryItem.item;

            text.text = chosenSeed.name.ToString();
        }

        if(seedObj == null)
        {
            chosenSeed = null;
        }
        
    }
}
