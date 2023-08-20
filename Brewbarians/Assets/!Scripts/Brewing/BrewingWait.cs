using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrewingWait : MonoBehaviour
{
    public BrewingManager manager;
    public Slider progressBar;
    public Image recipeItem;
    public TextMeshProUGUI quantityText;
    public int currentValue;

    public void Update()
    {
        //Debug.Log(progressBar.value);
        if (manager.chosenRecipe != null)
        {
            recipeItem.sprite = manager.chosenRecipe.Drink.image;
            quantityText.SetText(manager.quantity.ToString());

            progressBar.maxValue = manager.chosenRecipe.BrewTime;
            progressBar.value = currentValue;


            if (currentValue >= manager.chosenRecipe.BrewTime)
                manager.brewing = false;
        }
    }
}
