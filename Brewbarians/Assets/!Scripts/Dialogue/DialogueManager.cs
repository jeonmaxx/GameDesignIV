using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;

    [HideInInspector] public Message[] currentMessages;
    private Actor[] currentActors;
    [HideInInspector] public int activeMessage = 0;
    public bool isActive = false;

    public InputActionReference inputAction;
    private InputAction action;

    public AudioClip openSound;
    public AudioClip closeSound;

    private AudioSource source;

    public void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;
        action = inputAction.action;
        source = GetComponent<AudioSource>();
    }

    public void Update()
    {
        action.started += _ => OnSkip();
    }

    public void OnSkip()
    {
        if (isActive == true)
        {
            NextMessage();
        }
    }

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        if (!isActive)
        {
            source.clip = openSound;
            source.Play();
            currentMessages = messages;
            currentActors = actors;
            activeMessage = 0;                    
            DisplayMessage();
            backgroundBox.transform.localScale = Vector3.one;
            StartCoroutine(StartDialogue());
        }
    }

    public void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;

        AnimateTextColor();
    }

    public void NextMessage()
    {
        if (isActive)
        {
            activeMessage++;
            if (activeMessage < currentMessages.Length)
            {
                DisplayMessage();
            }
            else
            {
                source.clip = closeSound;
                source.Play();
                backgroundBox.transform.localScale = Vector3.zero;
                StartCoroutine(EndDialogue());
                Debug.Log("dialogue ended");
            }
        }
    }
    
    public void AnimateTextColor()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    private IEnumerator EndDialogue()
    {
        yield return new WaitForEndOfFrame();
        isActive = false;
        StopCoroutine(EndDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForEndOfFrame();
        isActive = true;
        StopCoroutine(StartDialogue());
    }
}
