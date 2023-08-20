using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldStates { None, Hoed, Wet}
public enum PlantStates { None, Phase01, Phase02, Phase03}
public class Planting : PlayerNear, IPointerDownHandler
{
    [Header("States")]
    //[ShowOnly]
    public FieldStates curFieldState;
    //[ShowOnly]
    public PlantStates curPlantState;
    
    [Header("Manager")]
    public HandManager handManager;
    public InventoryManager inventoryManager;
    public GameObject seedWheel;
    public TutorialDialogue tutorial;
    public PlayerMovement movement;

    [Header("Fields")]
    [SerializeField] private GameObject HoedField;
    [SerializeField] private GameObject WetField;
    [HideInInspector] public GameObject hoed;
    [HideInInspector] public GameObject wet;

    [Header("Planting")]
    public bool seedInHand;
    private ActionType handType;
    private bool inHand = false;
    public Seed seed;
    [SerializeField] private GameObject sign;
    private FarmSign farmSign;
    [SerializeField] private GameObject plant;
    public Item currentPlant;

    [Header("Tools")]
    [SerializeField] private Item shovelItem;
    [SerializeField] private Item waterItem;
    [SerializeField] private Item harvestItem;
    [SerializeField] private ToolSoundManager toolSoundManager;
    private AudioSource audioSource;

    public void Start()
    {
        farmSign = sign.GetComponent<FarmSign>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        CheckHand();
        PlantGrowing();
        currentPlant = farmSign.signSeed;
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CalcDistance();
        if (isPlayerNear)
        {
            ShovelField();
            WaterField();
            PlantField();
            Harvest();
            DestroyField();
        }
    }

    public void ShovelField()
    {
        if(curFieldState == FieldStates.None 
            && handManager.handItem == shovelItem)
        {
            audioSource.clip = toolSoundManager.shovelSounds[Random.Range(0, toolSoundManager.shovelSounds.Length)];
            StartCoroutine(PlayAnim("IsShoveling", 1.1f));
            hoed = Instantiate(HoedField, gameObject.transform.position, gameObject.transform.rotation, this.transform);
            curFieldState = FieldStates.Hoed;

            if (tutorial.state == TutorialState.Shovel)
            {
                tutorial.diaList[1].Done = true;
                tutorial.newState = true;
            }
        }
    }

    public void WaterField()
    {
        if (curFieldState == FieldStates.Hoed
            && handManager.handItem == waterItem
            && waterItem.currentWater > 0)
        {
            audioSource.clip = toolSoundManager.wateringSounds[Random.Range(0, toolSoundManager.wateringSounds.Length)];
            StartCoroutine(PlayAnim("IsWatering", 1.3f));
            wet = Instantiate(WetField, gameObject.transform.position, gameObject.transform.rotation, this.transform);
            curFieldState = FieldStates.Wet;
            waterItem.currentWater--;

            if (tutorial.state == TutorialState.Water)
            {
                tutorial.diaList[2].Done = true;
                tutorial.newState = true;
            }
        }
    }

    public void PlantField()
    {
        InventoryItem[] seedItem;
        seedItem = seedWheel.GetComponentsInChildren<InventoryItem>();

        if (tutorial.state == TutorialState.SeedPouch && handManager.handItem.actionType == ActionType.Plant)
        {
            tutorial.diaList[3].Done = true;
            tutorial.newState = true;
        }

        for (int i = 0; i < seedItem.Length; i++)
        {
            if ((curFieldState == FieldStates.Hoed
                || curFieldState == FieldStates.Wet)
                && curPlantState == PlantStates.None
                && handManager.handItem.actionType == ActionType.Plant
                && farmSign.signSeed != null)
            {
                seed = farmSign.signSeed.seed;

                if (seedItem[i] != null
                    && seedItem[i].item == farmSign.signSeed
                    && plant == null)
                {
                    audioSource.clip = toolSoundManager.bagSounds[Random.Range(0, toolSoundManager.bagSounds.Length)];
                    audioSource.Play();
                    //planted und rechnet einen seed count runter
                    plant = Instantiate(seed.Ph01, gameObject.transform.position, gameObject.transform.rotation, this.transform);
                    curPlantState = PlantStates.Phase01;
                    seedItem[i].count--;

                    if (tutorial.state == TutorialState.Seed)
                    {
                        tutorial.diaList[4].Done = true;
                        tutorial.newState = true;
                    }

                    //wenn der counter bei 0 ist, wird das childobject gelöscht
                    if (seedItem[i].count <= 0)
                    {
                        Destroy(seedItem[i].gameObject);
                        return;
                    }
                    else
                    {
                        seedItem[i].RefreshCount();
                        return;
                    }
                }
            }
        }
    }

    public void GivenField(FieldStates fieldState, int hoedPoints, int wetPoints)
    {
        Growing growing = GetComponent<Growing>();
        growing.currentHoedPoints = hoedPoints;
        growing.currentWetPoints = wetPoints;
        curFieldState = fieldState;

        switch(fieldState)
        {
            case FieldStates.Hoed:
                hoed = Instantiate(HoedField, gameObject.transform.position, gameObject.transform.rotation, transform);
                break;
            case FieldStates.Wet:
                hoed = Instantiate(HoedField, gameObject.transform.position, gameObject.transform.rotation, transform);
                wet = Instantiate(WetField, gameObject.transform.position, gameObject.transform.rotation, transform);
                break;
        }
    }

    public void GivenPlant(Seed newSeed, PlantStates plantStates, int growPoints)
    {
        Growing growing = GetComponent<Growing>();
        seed = newSeed;
        curPlantState = plantStates;
        growing.currentGrowPoints = growPoints;

        switch(plantStates)
        {
            case PlantStates.Phase01:
                plant = Instantiate(seed.Ph01, gameObject.transform.position, gameObject.transform.rotation, this.transform);
                break; 
            case PlantStates.Phase02:
                plant = Instantiate(seed.Ph02, gameObject.transform.position, gameObject.transform.rotation, this.transform);
                break;
            case PlantStates.Phase03:
                plant = Instantiate(seed.Ph03, gameObject.transform.position, gameObject.transform.rotation, this.transform);
                break;
        }
    }

    public void AddSavedSeed()
    {
        if (curPlantState == PlantStates.Phase01)
        {
            plant = Instantiate(seed.Ph01, gameObject.transform.position, gameObject.transform.rotation, this.transform);
        }
    }

    public void PlantGrowing()
    {
        if (curPlantState == PlantStates.Phase02)
        {
            Destroy(plant);
            plant = Instantiate(seed.Ph02, gameObject.transform.position, gameObject.transform.rotation, this.transform);
        }

        if (curPlantState == PlantStates.Phase03)
        {
            Destroy(plant);
            plant = Instantiate(seed.Ph03, gameObject.transform.position, gameObject.transform.rotation, this.transform);
        }
    }

    public void Harvest()
    {
        if(curPlantState == PlantStates.Phase03
            && handManager.handItem == harvestItem)
        {
            audioSource.clip = toolSoundManager.sickleSounds[Random.Range(0, toolSoundManager.sickleSounds.Length)];
            StartCoroutine(PlayAnim("IsSicheling", 1.2f));
            inventoryManager.AddItem(seed.Product);
            Destroy(plant);
            curPlantState = PlantStates.None;
            plant = null;

            if(tutorial.state == TutorialState.Harvest)
            {
                tutorial.currentHarvest++;
                if(tutorial.currentHarvest == tutorial.harvestAmount)
                {
                    tutorial.diaList[5].Done = true;
                    tutorial.newState = true;
                }               
            }
        }
    }

    public void DestroyField()
    {
        if (handManager.handItem == shovelItem
            && (curPlantState != PlantStates.None 
            && curPlantState != PlantStates.Phase03)
            && currentPlant.seed != seed)
        {
            Destroy(plant);
            curPlantState = PlantStates.None;
        }
    }

    public void CheckHand()
    {
        if (handManager.handItem != null)
        {
            handType = handManager.handItem.actionType;
            inHand = true;
        }
        else
            inHand = false;


        if ((int)handType == 3 && inHand)
            seedInHand = true;
        else if ((int)handType != 3 || !inHand)
            seedInHand = false;
    }

    public IEnumerator PlayAnim(string animName, float time)
    {
        movement.forbidToWalk = true;
        movement.animator.SetBool(animName, true);
        audioSource.Play();
        yield return new WaitForSeconds(time);
        movement.animator.SetBool(animName, false);
        movement.forbidToWalk = false;
        StopCoroutine(PlayAnim(animName, time));
    }


}
