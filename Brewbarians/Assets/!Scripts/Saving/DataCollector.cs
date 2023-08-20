using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class MainItems
{
    public Item MainItem;
    public int Counter;
    public MainItems(Item mainItem, int count)
    {
        MainItem = mainItem;
        Counter = count;
    }
}

[Serializable]
public class MainSeeds
{
    public Item MainSeed;
    public int Counter;
    public MainSeeds(Item mainSeed, int count)
    {
        MainSeed = mainSeed;
        Counter = count;
    }
}

[Serializable]
public class Tutorial
{
    public List<DialogueList> TutDia;
    public TutorialState State;
    public bool NewState;
    public bool ItemGiven;
    public bool EnablePopup;

    public Tutorial(List<DialogueList> tutDia, TutorialState state, bool newState, bool itemGiven, bool enablePopup)
    {
        TutDia = tutDia;
        State = state;
        NewState = newState;
        ItemGiven = itemGiven;
        EnablePopup = enablePopup;
    }
}

[Serializable]
public class BushLists
{
    public BushData BushData;
    public BushLists(BushData bushData)
    {
        BushData = bushData;
    }
}

public class DataCollector : MonoBehaviour
{
    [Header("Input")]
    public PlayerMovement playerMovement;
    public InventoryManager inventoryManager;
    public RecipeManager recipeManager;
    public PointsCollector pointsCollector;
    public FarmingManager farmingManager;
    public BrewStationManager brewStationManager;
    public TutorialDialogue tutorial;

    [Header("Data")]
    public Vector3 playerPosition;
    public List<MainItems> mainItems;
    public List<MainSeeds> mainSeeds;
    public List<Recipe> recipes;
    public Vector3 Points;
    private float farmPoints;
    private float brewPoints;
    private float dayPoints;
    public Vector2 scene;
    public List<Tutorial> tutorialList;

    private InventoryItem tmpInven;
    private int tmpCount;

    public List<BushLists> bushLists;
    public SaveBushes saveBushes;

    public void Start()
    {
        string filePath = Application.persistentDataPath + "/" + "scene.json";
        Debug.Log(filePath);

        if (File.Exists(filePath) && new FileInfo(filePath).Length > 0 )
            GiveData();
    } 

    public void CollectData()
    {
        //Player Position
        //playerPosition = playerMovement.gameObject.transform.position;

        //Main Items
        mainItems = new List<MainItems>();
        for (int i = 0; i < inventoryManager.inventorySlots.Length; i++) //Schleife so lange, wie es Slots gibt
        {
            if (inventoryManager.inventorySlots[i].transform.childCount != 0) // Wenn ein Slot nicht leer ist...
            {
                tmpInven = inventoryManager.inventorySlots[i].transform.GetChild(0).GetComponent<InventoryItem>(); //... nehm das erste Kind InventoryItem ...
                mainItems.Add(new MainItems(tmpInven.item, tmpInven.count)); // ... und tu es in die Liste
            }
            else
            {
                mainItems.Add(null); //Ansonsten add null (für leeren Slot)
            }
        }

        //Seeds
        mainSeeds = new List<MainSeeds>();
        for (int i = 0; i < inventoryManager.seedWheel.Length; i++)
        {
            if (inventoryManager.seedWheel[i].transform.childCount != 0)
            {
                tmpInven = inventoryManager.seedWheel[i].transform.GetChild(0).GetComponent<InventoryItem>();
                mainSeeds.Add(new MainSeeds(tmpInven.item, tmpInven.count));
            }
            else
            {
                mainSeeds.Add(null);
            }
        }

        //Recipes
        recipes = new List<Recipe>();
        if (recipeManager.recipeHolder.transform.childCount != 0 && recipeManager.recipeHolder.transform.childCount > recipes.Count)
        {   
            for (int i = 0; i < recipeManager.recipeHolder.transform.childCount; i++)
            {
                recipes.Add(recipeManager.recipeHolder.transform.GetChild(i).GetComponent<RecipeItem>().recipe);
            }
        }

        //GrowingPoints
        farmPoints = pointsCollector.addedFarmPoints;
        brewPoints = pointsCollector.addedBrewPoints;
        dayPoints = pointsCollector.dayTime;
        Points = new Vector3(farmPoints, brewPoints, dayPoints);

        //active Scene
        scene.x = SceneManager.GetActiveScene().buildIndex;

        //Bushes
        if (saveBushes != null)
        {
            saveBushes.CollectBushes();
            SavingBushes();
            SaveGameManager.SaveToJSON<BushLists>(bushLists, "bushes.json");
        }

        //Tutorial
        if (tutorial != null)
        {
            tutorialList.Add(new Tutorial(tutorial.diaList, tutorial.state, tutorial.newState, tutorial.itemGiven, tutorial.enablePopup));
            SaveGameManager.SaveToJSON<Tutorial>(tutorialList, "tutorial.json");
        }


        //Brewing
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            brewStationManager.AddData();
            SaveGameManager.SaveToJSON<BrewData>(brewStationManager.brewData, "brewing.json");
        }

        //Plants and Fields
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            farmingManager.AddToList();
            SaveGameManager.SaveToJSON<Plants>(farmingManager.plants, "plants.json");
            SaveGameManager.SaveToJSON<Fields>(farmingManager.fields, "fields.json");
            SaveGameManager.SaveToJSON<Item>(farmingManager.signSeed, "signSeeds.json");
        }


        //SaveGameManager.SaveToJSON(playerPosition, "position.json");
        SaveGameManager.SaveToJSON<MainItems>(mainItems, "items.json");
        SaveGameManager.SaveToJSON<MainSeeds>(mainSeeds, "seeds.json");
        SaveGameManager.SaveToJSON<Recipe>(recipes, "recipes.json");
        SaveGameManager.SaveToJSON(Points, "points.json");
        SaveGameManager.SaveToJSON(scene, "scene.json");
        
    }


    public void GiveData()
    {
        //playerPosition = SaveGameManager.ReadFromJSON<Vector3>("position.json");
        mainItems = SaveGameManager.ReadListFromJSON<MainItems>("items.json");
        mainSeeds = SaveGameManager.ReadListFromJSON<MainSeeds>("seeds.json");
        recipes = SaveGameManager.ReadListFromJSON<Recipe>("recipes.json");
        Points = SaveGameManager.ReadFromJSON<Vector3>("points.json");
        scene = SaveGameManager.ReadFromJSON<Vector2>("scene.json");

        farmingManager.plants = SaveGameManager.ReadListFromJSON<Plants>("plants.json");
        farmingManager.fields = SaveGameManager.ReadListFromJSON<Fields>("fields.json");
        farmingManager.signSeed = SaveGameManager.ReadListFromJSON<Item>("signSeeds.json");

        brewStationManager.brewData = SaveGameManager.ReadListFromJSON<BrewData>("brewing.json");

        tutorialList = SaveGameManager.ReadListFromJSON<Tutorial>("tutorial.json");

        //Changes Player Position
        //playerMovement.gameObject.transform.position = playerPosition;

        if (SceneManager.GetActiveScene().buildIndex == 1)
            farmingManager.UpdateFields();

        if(SceneManager.GetActiveScene().buildIndex == 2)
            brewStationManager.LoadData();

        LoadItems();
        LoadSeeds();
        LoadRecipes();

        bushLists = SaveGameManager.ReadListFromJSON<BushLists>("bushes.json");
        if(bushLists != null)
            GivingBushes();


        pointsCollector.addedFarmPoints = Points.x;
        pointsCollector.addedBrewPoints = Points.y;
        pointsCollector.dayTime = Points.z;

        if (tutorial != null)
        {
            for (int i = 0; i < tutorialList.Count; i++)
            {
                tutorial.diaList = tutorialList[i].TutDia;
                tutorial.state = tutorialList[i].State;
                tutorial.newState = tutorialList[i].NewState;
                tutorial.itemGiven = tutorialList[i].ItemGiven;
                tutorial.enablePopup = tutorialList[i].EnablePopup;
            }
        }
    }

    public void LoadItems()
    {
        DeleteItems();
        GiveItems();
    }

    public void LoadSeeds()
    {
        DeleteSeeds();
        GiveSeeds();
    }

    public void LoadRecipes()
    {
        DeleteRecipes();
        GiveRecipes();
    }

    private void DeleteItems()
    {
        for (int i = 0; i < inventoryManager.inventorySlots.Length; i++)
        {
            if (inventoryManager.inventorySlots[i].transform.childCount != 0)
            {
                Destroy(inventoryManager.inventorySlots[i].transform.GetChild(0).gameObject);
            }
        }
    }
    private void GiveItems()
    {
        for (int i = 0; i < mainItems.Count; i++)
        {
            InventoryItem inventoryItem = null;
            tmpCount = mainItems[i].Counter;
            for (int j = 0; j < tmpCount; j++)
            {
                if (j == 0)
                {
                    GameObject newItemGo = Instantiate(inventoryManager.inventoryItemPrefab, inventoryManager.inventorySlots[i].transform);
                    inventoryItem = newItemGo.GetComponent<InventoryItem>();
                    inventoryItem.InitialiseItem(mainItems[i].MainItem);
                }
                else if (j > 0)
                {
                    inventoryItem.count++;
                    inventoryItem.RefreshCount();
                }
            }
        }
    }

    private void DeleteSeeds()
    {
        for (int i = 0; i < inventoryManager.seedWheel.Length; i++)
        {
            if (inventoryManager.seedWheel[i].transform.childCount != 0)
            {
                Destroy(inventoryManager.seedWheel[i].transform.GetChild(0).gameObject);
            }
        }
    }
    private void GiveSeeds()
    {
        for (int i = 0; i < mainSeeds.Count; i++)
        {
            InventoryItem inventoryItem = null;
            tmpCount = mainSeeds[i].Counter;
            for (int j = 0; j < tmpCount; j++)
            {
                if (j == 0)
                {
                    GameObject newItemGo = Instantiate(inventoryManager.inventoryItemPrefab, inventoryManager.seedWheel[i].transform);
                    inventoryItem = newItemGo.GetComponent<InventoryItem>();
                    inventoryItem.InitialiseItem(mainSeeds[i].MainSeed);
                }
                else if (j > 0)
                {
                    inventoryItem.count++;
                    inventoryItem.RefreshCount();
                }
            }
        }
    }

    private void DeleteRecipes()
    {
        for (int i = 0; i < recipeManager.recipeHolder.transform.childCount && recipeManager.recipeHolder.transform.childCount != 0; i++)
        {
            Destroy(recipeManager.recipeHolder.transform.GetChild(i).gameObject);
        }
    }
    private void GiveRecipes()
    {
        for (int j = 0; j < recipes.Count; j++)
        {
            recipeManager.AddRecipe(recipes[j]);
        }
    }

    public void SavingBushes()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 3:
                bushLists[0].BushData = saveBushes.bushData;
                break;
            case 4:
                bushLists[1].BushData = saveBushes.bushData;
                break;
            case 1:
                bushLists[2].BushData = saveBushes.bushData;
                break;
        }
    }

    public void GivingBushes()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 3:
                saveBushes.bushData = bushLists[0].BushData;
                BushBool(0);
                break;
            case 4:
                saveBushes.bushData = bushLists[1].BushData;
                BushBool(1);
                break;
            case 1:
                saveBushes.bushData = bushLists[2].BushData;
                BushBool(2);
                break;
        }
    }

    private void BushBool(int num)
    {
        if (bushLists[num].BushData != null)
        {
            for (int i = 0; i < bushLists[num].BushData.Empty.Count; i++)
            {
                saveBushes.bushes[i].emptyBool = bushLists[num].BushData.Empty[i];
            }
        }
    }
}
