using System;
using System.Collections;
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

    public void Start()
    {
        action = inputAction.action;
    }

    private void Update()
    {
        action.started += _ => OnInteract();

        CalcDistance();

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
    }

    public void OnInteract()
    {
        if (isPlayerNear)
        {
            trigger.StartDialogue();
        }
    }

    public void NextStage(QuestStage nextStage, bool done)
    {
        if (done)
            currentStage = nextStage;
    }
}
