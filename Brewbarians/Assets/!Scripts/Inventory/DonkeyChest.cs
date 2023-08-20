using UnityEngine;
using UnityEngine.InputSystem;

public class DonkeyChest : PlayerNear
{
    public InputActionReference inputAction;
    private InputAction action;    
    public GameObject chest;
    public bool chestOpen;
    public InteractableSign interactableSign;


    private void Start()
    {
        action = inputAction.action;
        chest.GetComponent<Canvas>().enabled = false;
    }

    private void Update()
    {
        CalcDistance();

        if (isPlayerNear)
        {
            action.started += _ => OnOpenChest();
            interactableSign.gameObject.SetActive(true);
            interactableSign.ShowInteraction();
            if (!chestOpen)
            {
                chest.GetComponent<Canvas>().enabled = false;
            }
            else
            {
                chest.GetComponent<Canvas>().enabled = true;
            }
        }
        else
        {
            interactableSign.gameObject.SetActive(false);
            chest.GetComponent<Canvas>().enabled = false;
            chestOpen = false;
        }

    }
    private void OnOpenChest()
    {
        if (chestOpen)
        {
            chestOpen = false;
        }
        else if (!chestOpen)
        {
            chestOpen = true;
        }

    }
}
