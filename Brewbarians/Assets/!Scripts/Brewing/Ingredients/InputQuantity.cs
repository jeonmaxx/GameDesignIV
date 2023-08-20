using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputQuantity : MonoBehaviour
{
    public OpenBrewing brewing;
    public TMP_InputField inputField;
    private int chosenQuant = 1;
    public BrewingManager brewingManager;

    public void Update()
    {
        CheckInteractable();
        CheckQuant();

        if (chosenQuant <= 5)
        {
            brewingManager.quantity = chosenQuant;
        }
        else if(chosenQuant > 5)
        {
            inputField.text = "5";
            chosenQuant = 5;
            brewingManager.quantity = chosenQuant;
        }
    }

    private void CheckInteractable()
    {
        if (brewing.menuOpen)
        {
            inputField.interactable = true;
        }
        else
            inputField.interactable = false;
    }

    private void CheckQuant()
    {
        if(inputField.text != null) 
            //chosenQuant = System.Convert.ToInt32(inputField.text);
            int.TryParse(inputField.text, out chosenQuant);
    }
}
