using UnityEngine;
using UnityEngine.InputSystem;

public enum QteKind { Bar }
public class QTE : MonoBehaviour
{
    [SerializeField] private QteKind kind;
    [SerializeField] private RectTransform goodZone;
    [SerializeField] private RectTransform player;
    [SerializeField] private int playerSpeed;
    [SerializeField] private Vector2 randomSpeedMinMax;
    [SerializeField] private Vector2 xPosition;
    [SerializeField] private Vector2 playerZone;
    private float xPos;
    [SerializeField] private Vector2 position;
    public bool moving = false;
    private bool movingRight = true;
    public bool done = false;
    [SerializeField] private PlayerQte playerQte;

    public BrewingManager manager;

    public InputActionReference inputAction;
    private InputAction action;

    void Start()
    {
        ChangePos();
        transform.localScale = Vector3.zero;

        action = inputAction.action;
    }

    public void Update()
    {
        action.started += _ => OnQte();

        if (moving)
        {
            transform.localScale = Vector3.one;
            PlayerMoving();
        }
        else
            transform.localScale = Vector3.zero;
    }

    public void QteMethode()
    {
        moving = true;
        done = false;
        RandomSpeed();
        ChangePos();
    }

    private void ChangePos()
    {
        switch (kind)
        {
            case QteKind.Bar:
                ZonePos(xPosition);
                goodZone.anchoredPosition = new Vector3(xPos, goodZone.anchoredPosition.y, 0);
                break;
            default: break;
        }
    }

    private float ZonePos(Vector2 position)
    {
        return xPos = Random.Range(position.x, position.y);
    }

    private void RandomSpeed()
    {
        playerSpeed = Random.Range((int)randomSpeedMinMax.x, (int)randomSpeedMinMax.y) * 10;
    }

    private void PlayerMoving()
    {
        //einfache Links rechts bewegung

        position = player.anchoredPosition;
        if (position.x <= playerZone.x)
        {
            movingRight = true;
        }
        else if (position.x >= playerZone.y)
        {
            movingRight = false;
        }

        if (movingRight)
        {
            position.x += playerSpeed * Time.deltaTime;
        }
        else if(!movingRight)
        {
            position.x -= playerSpeed * Time.deltaTime;
        }

        player.anchoredPosition = position;
    }

    //Brewing Manager hat auch ein OnQte
    public void OnQte()
    {
        if (moving)
            moving = false;

        if (manager.checking)
        {
            if (playerQte.greenZone)
            {
                Debug.Log("Green Zone hit!");
                manager.bonusPoints += 2;
                done = true;
            }
            else if (playerQte.yellowZone)
            {
                Debug.Log("Yellow Zone hit!");
                manager.bonusPoints += 1;
                done = true;
            }
            else
            {
                Debug.Log("Blue Zone hit!");
                done = true;
            }
        }
    }
}
