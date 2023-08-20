using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum BrewingStates { Recipe, IngreOne, IngreTwo, Waiting }
public class OpenBrewing : PlayerNear
{
    public RectTransform[] menus;
    public RectTransform currentRect;
    public BrewingStates state;
    //curentrect braucht einen enum

    public bool menuOpen = false;
    public bool choosing = false;

    public PlayerMovement movement;
    public OpenInventory inventory;

    public TutorialDialogue tutorial;

    public InputActionReference inputAction;
    private InputAction action;

    public InteractableSign interactableSign;

    public void Start()
    {
        currentRect = menus[0];
        for(int i = 0;  i < menus.Length; i++)
        {
            menus[i].transform.localScale = Vector3.zero;
        }
        action = inputAction.action;
    }

    public void Update()
    {
        CalcDistance();

        action.started += _ => OnInteract();

        if (isPlayerNear)
        {
            interactableSign.gameObject.SetActive(true);
            interactableSign.ShowInteraction();
        }
        else
            interactableSign.gameObject.SetActive(false);

        if (menuOpen && isPlayerNear)
        {
            currentRect.transform.localScale = Vector3.one;
            movement.forbidToWalk = true;
            movement.enabled = false;

            foreach (RectTransform rectTransform in menus)
            {
                if(rectTransform != currentRect)
                {
                    rectTransform.transform.localScale = Vector3.zero;
                }
            }
        }
        else if (!menuOpen)
        {
            currentRect.transform.localScale = Vector3.zero;
            movement.enabled = true;
            movement.forbidToWalk = false;            
        }

        if(state == BrewingStates.Waiting)
        {
            if (tutorial.state == TutorialState.Qte)
            {
                tutorial.trigger.passivePassed = false;
                tutorial.diaList[9].Done = true;
                tutorial.newState = true;
                tutorial.enablePopup = true;
            }
        }

        currentRect = menus[(int)state];
    }

    public void OnInteract()
    {
        if (isPlayerNear && !menuOpen || choosing)
        {
            menuOpen = true;
            tutorial.enablePopup = true;

            if(tutorial.state == TutorialState.GoToMachine)
            {
                tutorial.diaList[7].Done = true;
                tutorial.newState = true;
                tutorial.trigger.passivePassed = false;
            }
        }
        else if (menuOpen)
        {
            menuOpen = false;
        }
    }

    public void Open(bool inventoryToo)
    {
        if (inventoryToo)
        {
            inventory.inventoryActive = false;
            choosing = false;
        }

        menuOpen = true;
    }

    public void Close(bool inventoryToo)
    {
        if(inventoryToo)
        {
            inventory.inventoryActive = true;
            choosing = true;
        }

        menuOpen = false;
    }


}
