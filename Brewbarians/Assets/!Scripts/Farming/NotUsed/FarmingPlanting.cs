using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.EventSystems;

public class FarmingPlanting : MonoBehaviour, IPointerDownHandler
{
    public HandManager handManager;
    public InventoryManager inventoryManager;
    private ActionType handType;
    private bool inHand = false;
    public bool seedInHand;
    private PlantState plantState;
    private FieldState fieldState;
    private Seed seed;
    public GameObject sign;
    private FarmSign farmSign;

    public void Start()
    {
        farmSign = sign.GetComponent<FarmSign>();
        fieldState = GetComponent<FieldState>();
        plantState = GetComponent<PlantState>();
    }

    public void Update()
    {
        CheckHand();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlantField();
    }

    public void PlantField()
    {
        if (farmSign.signSeed != null)
        {
            seed = farmSign.signSeed.seed;
            if ((fieldState.currentFieldState == fieldState.fieldStates[1]
                || fieldState.currentFieldState == fieldState.fieldStates[2])
                && plantState.currentPlantState == plantState.plantStates[0]
                && handManager.handItem == farmSign.signSeed)
            {
                Instantiate(seed.Ph01, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
                plantState.currentPlantState = plantState.plantStates[1];
                Item receivedItem = inventoryManager.GetSelectedItem(true);
            }
        }
    }


    public void CheckHand()
    {
        if (handManager.handItem != null)
        {
            handType = handManager.handItem.actionType;
            inHand = true;
        }
        else
            inHand = false;


        if ((int)handType == 2 && inHand)
            seedInHand = true;
        else if ((int)handType != 2 || !inHand)
            seedInHand = false;
    }
}
