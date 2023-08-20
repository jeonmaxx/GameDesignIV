using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    public bool passivePassed;
    private DialogueManager dialogue;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
    }

    public void PassiveDialogue()
    {
        dialogue = FindObjectOfType<DialogueManager>();
        if (!passivePassed)
        {
            if (!dialogue.isActive)
                StartDialogue();
            StartCoroutine(PassiveNext());
        }
    }

    IEnumerator PassiveNext()
    {
        yield return new WaitForSeconds(3);
        dialogue.activeMessage++;
        if (dialogue.activeMessage < dialogue.currentMessages.Length)
        {
            dialogue.DisplayMessage();
        }
        else
        {
            dialogue.backgroundBox.LeanScale(Vector3.zero, 0).setEaseInOutExpo();
            dialogue.isActive = false;
            passivePassed = true;
        }
    }
}

[System.Serializable]
public class Message
{
    public int actorId;
    public string message;
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite sprite;
}
