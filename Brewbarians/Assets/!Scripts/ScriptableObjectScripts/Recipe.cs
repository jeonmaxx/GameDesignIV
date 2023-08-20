using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Recipe")]
public class Recipe : ScriptableObject
{
    public string DrinkName;
    public int Product1Amount;
    public Item Product1;
    public int Product2Amount;
    public Item Product2;
    public Item Drink;
    public int BrewTime;
}
