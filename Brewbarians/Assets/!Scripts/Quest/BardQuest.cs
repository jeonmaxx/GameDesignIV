using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public Animator anim;
    private bool stageChecked;
    public Vector3 afterQuestPos;
    public Recipe givenRecipe;
    public RecipeManager recipeManager;
    public InteractableSign interactableSign;

    public void Start()
    {
        action = inputAction.action;
    }

    private void Update()
    {
        action.started += _ => OnInteract();

        if (currentStage == QuestStage.Done && !stageChecked)
        {
            transform.position = afterQuestPos;
            anim.SetBool("questDone", true);
            stageChecked = true;
        }
        else if(currentStage != QuestStage.Done && !stageChecked)
        {
            stageChecked = true;
        }

        CalcDistance();

        if (newStage && transform.childCount == 1)
        {
            Instantiate(exclaPrefab, transform);
        }
        else if (!newStage && transform.childCount != 1)
        {
            Destroy(transform.GetChild(1).gameObject);
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

        if (currentStage != QuestStage.Done)
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
                    recipeManager.AddRecipe(givenRecipe);
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
