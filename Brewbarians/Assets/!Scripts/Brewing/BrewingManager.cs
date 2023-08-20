using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BrewingManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public BrewingWait wait;
    public TutorialDialogue tutorial;

    [HideInInspector] public Recipe chosenRecipe;
    [HideInInspector] public Item itemOne;
    [HideInInspector] public Item itemTwo;

    [HideInInspector] public Item neededOne;
    [HideInInspector] public Item neededTwo;

    public TMP_Text ingreOneShow;
    public TMP_Text ingreTwoShow;
    public TMP_Text ingreOneAmount;
    public TMP_Text ingreTwoAmount;

    public TMP_Text ingreOneText;
    public TMP_Text ingreTwoText;

    public bool allThere = false;
    private bool enoughOne = false;
    private bool enoughTwo = false;

    public GameObject itemOneObj;
    public GameObject itemTwoObj;
    public InventoryItem inventoryItemOne;
    public InventoryItem inventoryItemTwo;

    public OpenBrewing open;
    public bool brewing = false;

    public int quantity;

    public bool checking = false;

    public QTE qte;

    public int bonusPoints;

    public InputActionReference inputAction;
    private InputAction action;

    private void Start()
    {
        action = inputAction.action;
        open = GetComponent<OpenBrewing>();
    }

    public void Update()
    {
        CheckRecipe();
        CheckIngredients();
        action.started += _ => OnQte();

        if(!brewing && tutorial.state == TutorialState.Brewing)
        {
            tutorial.diaList[10].Done = true;
            tutorial.newState = true;
        }
    }

    public void CheckRecipe()
    {
        if(chosenRecipe != null)
        {
            neededOne = chosenRecipe.Product1;
            ingreOneText.SetText(chosenRecipe.Product1.itemName);
            neededTwo = chosenRecipe.Product2;
            ingreTwoText.SetText(chosenRecipe.Product2.itemName);
            ingreOneShow.SetText(chosenRecipe.Product1.itemName);
            ingreTwoShow.SetText(chosenRecipe.Product2.itemName);
            ingreOneAmount.SetText((chosenRecipe.Product1Amount * quantity).ToString());
            ingreTwoAmount.SetText((chosenRecipe.Product2Amount * quantity).ToString());
        }
    }

    //nimmt bei BrewStation02 den qte nicht an (??????)
    public void CheckIngredients()
    {
        if (chosenRecipe != null)
        {
            if (itemOne == neededOne)
            {
                inventoryItemOne = itemOneObj.GetComponentInChildren<InventoryItem>();
            }
            if(itemTwo == neededTwo)
            {
                inventoryItemTwo = itemTwoObj.GetComponentInChildren<InventoryItem>();
            }

            int amountOne = chosenRecipe.Product1Amount * quantity;
            int amountTwo = chosenRecipe.Product2Amount * quantity;

            if (inventoryItemOne != null)
            {
                if (inventoryItemOne.count >= amountOne)
                    enoughOne = true;
                else
                    enoughOne = false;
            }

            if (inventoryItemTwo != null)
            {
                if (inventoryItemTwo.count >= amountTwo)
                    enoughTwo = true;
                else
                    enoughTwo = false;
            }
        }
    }

    public void ConfirmRecipeButton(int brew)
    {
        if (brew == 0 && chosenRecipe != null && quantity != 0)
        {
            open.state = BrewingStates.IngreOne;
            ChangingScreen(1);
        }

        else if (brew == 1 && itemOne == neededOne && enoughOne)
        {
            if (tutorial.state == TutorialState.ChooseRec)
            {
                tutorial.diaList[8].Done = true;
                tutorial.newState = true;
                tutorial.trigger.passivePassed = false;
                tutorial.enablePopup = true;
            }
            checking = true;
            qte.QteMethode();
        }

        else if (brew == 2 && itemTwo == neededTwo && enoughTwo)
        {
            checking = true;
            qte.QteMethode();
        }
    }

    public void OnQte()
    {
        if(checking)
        {
            Debug.Log("checking");
            StartCoroutine(WaitQte());
        }
    }
    private void ChangingScreen(int menu)
    {
        open.Close(false);
        open.currentRect = open.menus[menu];
        open.Open(false);
    }

    public void BrewButton()
    {      
        if (!brewing)
        {
            for (int i = 0; i < (quantity + bonusPoints); i++)
            {
                inventoryManager.AddItem(chosenRecipe.Drink);
                if (tutorial.state == TutorialState.Ready)
                {
                    tutorial.diaList[11].Done = true;
                    tutorial.newState = true;
                }
            }

            if (inventoryItemOne.count > 0 && inventoryItemTwo.count > 0)
            {
                inventoryItemOne.count -= (chosenRecipe.Product1Amount * quantity);
                inventoryItemTwo.count -= (chosenRecipe.Product2Amount * quantity);

                RefreshItems(inventoryItemOne);
                RefreshItems(inventoryItemTwo);

                //Items die noch drin sind werden danach zurück gegeben
                if (inventoryItemOne.count != 0)
                {
                    int temp = inventoryItemOne.count;
                    for (int i = 0; i < temp; i++)
                    {
                        inventoryManager.AddItem(itemOne);
                        inventoryItemOne.count--;
                    }
                    RefreshItems(inventoryItemOne);
                }

                if (inventoryItemTwo.count != 0)
                {
                    int temp = inventoryItemTwo.count;
                    for (int i = 0; i < temp; i++)
                    {
                        inventoryManager.AddItem(itemTwo);
                        inventoryItemTwo.count--;
                    }
                    RefreshItems(inventoryItemTwo);
                }
            }

            brewing = false;
            bonusPoints = 0;
            wait.currentValue = 0;
            wait.progressBar.value = 0;
            open.state = BrewingStates.Recipe;
            ChangingScreen(0);
        }
    }

    private void RefreshItems(InventoryItem item)
    {
        if (item.count <= 0)
        {
            Destroy(item.gameObject);
        }
        else
        {
            item.RefreshCount();
        }
    }

   private IEnumerator WaitQte()
    {
        yield return new WaitForEndOfFrame();
        if (open.currentRect == open.menus[1])
        {
            open.state = BrewingStates.IngreTwo;
            ChangingScreen(2);
            Debug.Log("changed screen");
        }
        else if (open.currentRect == open.menus[2])
        {
            open.state = BrewingStates.Waiting;
            ChangingScreen(3);
            brewing = true;
        }
        checking = false;
        StopCoroutine(WaitQte());
    }
}