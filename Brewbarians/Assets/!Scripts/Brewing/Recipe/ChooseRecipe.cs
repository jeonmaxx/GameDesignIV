using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseRecipe : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject brewStation;
    [HideInInspector] public OpenBrewing openBrewing;
    [HideInInspector] public BrewingManager brewingManager;
    [SerializeField] private GameObject manager;
    [SerializeField] private GameObject recipeHolder;
    [HideInInspector] public List<RecipeClicked> recipeClicked;
    public Recipe chosenRecipe;
    private OpenInventory inventory;
    [HideInInspector] public Image slotImage;

    [HideInInspector] public bool recipeChosen = false;

    private void Start()
    {
        slotImage = GetComponent<Image>();
        inventory = manager.GetComponent<OpenInventory>();
        openBrewing = brewStation.GetComponent<OpenBrewing>();
        brewingManager = brewStation.GetComponent<BrewingManager>();        
    }

    private void Update()
    {
        if(openBrewing.choosing)
        {
            
            for(int i = 0; i < recipeClicked.Count; i++)
            {
                recipeClicked[i].clickable = true;
            }
        }
        //else if(!openBrewing.choosing)
        //{
        //    for (int i = 0; i < recipeClicked.Count; i++)
        //    {
        //        recipeClicked[i].clickable = false;
        //    }
        //}

        brewingManager.chosenRecipe = chosenRecipe;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(openBrewing.menuOpen)
        {
            inventory.windows = Windows.Recipe;
            openBrewing.Close(true);
            recipeChosen = false;

            for (int i = 0; i < recipeClicked.Count; i++)
            {
                recipeClicked[i].clickable = false;
            }
        }
    }
}


