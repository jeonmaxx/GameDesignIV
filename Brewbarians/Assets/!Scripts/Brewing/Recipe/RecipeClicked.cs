using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeClicked : MonoBehaviour, IPointerClickHandler
{
    public bool clickable = false;
    public GameObject[] recipeSlot;
    public ChooseRecipe choose;
    private RecipeItem recipeItem;

    public Recipe rec;

    public GameObject manager;
    public GameObject[] brewingStation;
    public OpenBrewing openBrewing;

    public void Update()
    {
        if (brewingStation.Length > 0 && brewingStation[0] != null)
        {
            if (brewingStation[0].GetComponent<OpenBrewing>().menuOpen)
            {
                openBrewing = brewingStation[0].GetComponent<OpenBrewing>();
                choose = recipeSlot[0].GetComponent<ChooseRecipe>();
            }
            else if (brewingStation[1].GetComponent<OpenBrewing>().menuOpen)
            {
                openBrewing = brewingStation[1].GetComponent<OpenBrewing>();
                choose = recipeSlot[1].GetComponent<ChooseRecipe>();
            }
        }  
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (openBrewing != null && clickable && recipeSlot != null)
        {
            recipeItem = GetComponent<RecipeItem>();
            choose.chosenRecipe = recipeItem.recipe;
            choose.recipeChosen = true;

            Debug.Log("recipe added");

            if (!openBrewing.menuOpen && choose.recipeChosen)
            {
                openBrewing.Open(true);
                choose.slotImage.sprite = choose.chosenRecipe.Drink.image;
            }
        }
    }
}
