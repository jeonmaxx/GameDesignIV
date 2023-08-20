using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeManager : MonoBehaviour
{
    public GameObject recipeHolder;
    public GameObject recipePrefab;
    private RecipeItem rec;
    public bool brewingStaion = false;
    [Header ("Only if brewingStation true")]
    [SerializeField] private ChooseRecipe[] chooseRecipe;
    [SerializeField] private GameObject[] recipeSlot;
    public GameObject[] brewingStation;

    public void AddRecipe(Recipe recipe)
    {
        rec = recipePrefab.GetComponent<RecipeItem>();
        
        rec.recipe = recipe;

        rec.drinkName.text = recipe.DrinkName;
        rec.drinkSprite.sprite = recipe.Drink.image;

        rec.prodcut01Name.text = recipe.Product1.itemName;
        rec.product01Amount.text = recipe.Product1Amount.ToString();
        rec.product01Sprite.sprite = recipe.Product1.image;

        rec.prodcut02Name.text = recipe.Product2.itemName;
        rec.product02Amount.text = recipe.Product2Amount.ToString();
        rec.product02Sprite.sprite = recipe.Product2.image;

        GameObject obj = Instantiate(recipePrefab, recipeHolder.transform.position, recipeHolder.transform.rotation, recipeHolder.transform);
        if(brewingStaion) 
        {
            for (int i = 0; i < brewingStation.Length; i++)
            {
                RecipeClicked recipeClicked = obj.GetComponent<RecipeClicked>();
                chooseRecipe[i].recipeClicked.Add(recipeClicked);
                recipeClicked.manager = gameObject;
                recipeClicked.brewingStation[i] = brewingStation[i];
                recipeClicked.recipeSlot[i] = recipeSlot[i];
            }
        }
    }

}
