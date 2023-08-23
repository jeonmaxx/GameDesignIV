using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[Serializable]
public class QuestList
{
    public QuestStage Stage;
    public Message[] Messages;
    public bool Done;

    public QuestList(QuestStage stage, Message[] messages, bool done)
    {
        Stage = stage;
        Messages = messages;
        Done = done;
    }
}
public enum QuestStage { Introduction, QuestRepeat, GiveItem, Done }
public class BardQuest : PlayerNear
{
    public DialogueTrigger trigger;
    public DialogueManager manager;
    [ShowOnly] public QuestStage currentStage;
    public List<QuestList> questList;
    public HandManager handManager;
    public Item searchedItem;
    public InputActionReference inputAction;
    private InputAction action;
    public GameObject exclaPrefab;
    public bool newStage;
    public InventoryManager inventoryManager;
    public GameObject ItemObj;

    public void Start()
    {
        action = inputAction.action;
    }

    private void Update()
    {
        action.started += _ => OnInteract();
        CalcDistance();

        if (newStage && transform.childCount == 0)
        {
            Instantiate(exclaPrefab, transform);
        }
        else if (!newStage && transform.childCount != 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }



        for (int i = 0; i < questList.Count; i++)
        {
            if (currentStage == questList[i].Stage)
            {
                trigger.messages = questList[i].Messages;
            }

            if (questList[i].Done)
            {
                NextStage((QuestStage)(i + 1), questList[i].Done);
            }
        }

        if(currentStage == QuestStage.Introduction)
        {
            newStage = true;
        }

        if (currentStage == QuestStage.QuestRepeat)
        {
            for (int i = 0; i < inventoryManager.inventorySlots.Length; i++)
            {
                if (inventoryManager.inventorySlots[i].transform.childCount != 0 && inventoryManager.inventorySlots[i].transform.GetChild(0).GetComponent<InventoryItem>().item == searchedItem)
                {
                    ItemObj = inventoryManager.inventorySlots[i].transform.GetChild(0).gameObject;
                    questList[1].Done = true;
                    newStage = true;
                }

            }
        }
    }

    public void OnInteract()
    {   
        if (isPlayerNear)
        {
            trigger.StartDialogue();
            switch (currentStage)
            {
               case QuestStage.Introduction:
                    newStage = false;
                    questList[0].Done = true;
                    break;
                case QuestStage.QuestRepeat:
                    break;
                case QuestStage.GiveItem:
                    newStage = false;
                    Destroy(ItemObj.gameObject);
                    questList[2].Done = true;
                    break;
                case QuestStage.Done:
                    break;
            }
        }
    }

    public void NextStage(QuestStage nextStage, bool done)
    {
        if (done)
            currentStage = nextStage;
    }
}
