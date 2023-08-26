using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueNpc : PlayerNear
{
    public DialogueTrigger trigger;

    public InputActionReference inputAction;
    private InputAction action;

    public InteractableSign interactableSign;

    public void Start()
    {
        action = inputAction.action;
    }

    private void Update()
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
    }

    public void OnInteract()
    {
        if (isPlayerNear)
        {
            trigger.StartDialogue();
            Debug.Log(trigger.messages.Length);
        }
    }

    
}
