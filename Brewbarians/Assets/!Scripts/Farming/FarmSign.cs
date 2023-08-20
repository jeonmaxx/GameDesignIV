using UnityEngine;
using UnityEngine.InputSystem;

public class FarmSign : PlayerNear
{
    public HandManager handManager;
    private ActionType handType;
    private bool inHand = false;
    public bool seedInHand;
    public RectTransform seedWheel;
    private SeedWheelManager seedWheelManager;

    public Item signSeed;

    public SpriteRenderer signSprite;

    private Color color;

    public bool wheelOpen = false;

    public InputActionReference inputAction;
    private InputAction action;

    public InteractableSign interactableSign;

    public void Start()
    {
        seedWheel.transform.localScale = Vector3.zero;
        color = signSprite.color;
        action = inputAction.action;
    }

    public void Update()
    {
        CheckHand();
        CalcDistance();

        action.started += _ => OnInteract();

        if (handManager.handItem != null && (isPlayerNear && handManager.handItem.actionType == ActionType.Plant))
        {
            interactableSign.gameObject.SetActive(true);
            interactableSign.ShowInteraction();
        }
        else
            interactableSign.gameObject.SetActive(false);

        if (wheelOpen && !isPlayerNear)
        {
            signSeed = seedWheelManager.chosenSeed;
            seedWheel.transform.localScale = Vector3.zero;
            wheelOpen = false;
        }

        if (signSeed != null)
        {
            signSprite.sprite = signSeed.image;
            signSprite.color = new Color(color.r, color.g, color.b, 100);
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

    public void OnInteract()
    {
        if (isPlayerNear && seedInHand && !wheelOpen)
        {
            seedWheel.transform.localScale = Vector3.one;
            seedWheelManager = seedWheel.GetComponent<SeedWheelManager>();
            signSeed = seedWheelManager.chosenSeed;
            if (signSeed != null)
            {
                signSprite.sprite = signSeed.image;
                signSprite.color = new Color(color.r, color.g, color.b, 100);
            }
            wheelOpen = true;
        }

        else if (wheelOpen)
        {
            signSeed = seedWheelManager.chosenSeed;
            if (signSeed != null)
            {
                signSprite.sprite = signSeed.image;
                signSprite.color = new Color(color.r, color.g, color.b, 100);
            }
            seedWheel.transform.localScale = Vector3.zero;
            wheelOpen = false;
        }
    }
}
