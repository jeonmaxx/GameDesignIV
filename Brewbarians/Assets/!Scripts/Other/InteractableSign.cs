using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableSign : MonoBehaviour
{
    public TextMeshPro text;
    public Text bindingText;
    public string action;
    public string tmpText;

    public void Start()
    {
        if(bindingText != null)
            tmpText = $"[{bindingText.text}] " + action;
    }

    public void ShowInteraction()
    {
        text.text = tmpText;
    }
}
