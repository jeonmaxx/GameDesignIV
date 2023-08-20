using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayTime : MonoBehaviour
{
    public float maxDayTime = 10;
    public float currentTime;
    public Transform TimeArrow;
    public Image nightTime;

    public bool playAmbiente;

    public AudioClip dayMusic;
    public AudioClip eveningMusic;

    public PointsCollector collector;
    public float night;

    private bool coroutineDone;

    public GameObject bed;
    public int SceneIndexBed;

    public Image blackScreen;
    public TextMeshProUGUI wakingUpText;
    private float alpha;
    private bool passedOut;

    private bool startCheck;

    public SceneTester loadNext;
    private GameObject player;
    private PlayerMovement playerMovement;

    private bool sleeping;

    public AudioSource source;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>(); 
        coroutineDone = false;
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);
        if(bed != null )
            wakingUpText.color = new Color(wakingUpText.color.r, wakingUpText.color.g, wakingUpText.color.b, 0);
        startCheck = false;   
    }

    private void FixedUpdate()
    {      
        if (!coroutineDone)
            StartCoroutine(ChangeNightSky());

        if (bed != null && !startCheck && !passedOut)
        {
            currentTime = collector.dayTime;
            if (collector.dayTime == maxDayTime)
            {
                StartCoroutine(WakingUpWaiting());
            }
            else
            {
                startCheck = true;
                passedOut = false;
                Debug.Log(currentTime);
            }
        }
        else if (passedOut && bed != null && !startCheck)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
            wakingUpText.color = new Color(wakingUpText.color.r, wakingUpText.color.g, wakingUpText.color.b, alpha);
            if (alpha > 0)
            {
                alpha -= Time.deltaTime * 0.3f;
            }
            else if (alpha <= 0)
            {
                startCheck = true;
                passedOut = false;
            }
        }
        else if (bed == null && !startCheck)
        {
            startCheck = true;
        }

    }

    private IEnumerator WakingUpWaiting()
    {
        alpha = 1;
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1);
        wakingUpText.color = new Color(wakingUpText.color.r, wakingUpText.color.g, wakingUpText.color.b, 1);        
        player.transform.position = bed.transform.position;
        collector.dayTime = 0;
        currentTime = 0; 
        yield return new WaitForSeconds(2.5f);
        passedOut = true;
    }

    public void Update()
    {
        if (currentTime < (maxDayTime * 0.65f) && playAmbiente)
        {
            source.clip = dayMusic;
            if(!source.isPlaying)
                source.Play();
        }
        else if (currentTime > (maxDayTime * 0.65f) && playAmbiente)
        {
            source.clip = eveningMusic;
            if (!source.isPlaying)
                source.Play();
        }

        if (!coroutineDone)
            StartCoroutine(ChangeNightSky());

        if (currentTime < maxDayTime)
            currentTime = collector.dayTime;
        else
            collector.dayTime = maxDayTime;

        ArrowRotation();

        if (currentTime >= (maxDayTime * 0.65f) && coroutineDone)
        {
            if (night <= ((currentTime * maxDayTime) / 130))
                night += (Time.deltaTime * 0.1f);

            nightTime.color = new Color(nightTime.color.r, nightTime.color.g, nightTime.color.b, night);
        }

        if(currentTime == 0)
        {
            night = 0;
            nightTime.color = new Color(nightTime.color.r, nightTime.color.g, nightTime.color.b, night);
        }

        if(startCheck)
            PassOut();
    }

    public void ArrowRotation()
    {
        float angle = -(currentTime * (180 / maxDayTime));

        TimeArrow.eulerAngles = new Vector3(
        TimeArrow.transform.eulerAngles.x,
        TimeArrow.transform.eulerAngles.y,
        angle);
    }

    public IEnumerator ChangeNightSky()
    {
        yield return new WaitForEndOfFrame();
        if (currentTime > (maxDayTime * 0.65f))
        {
            night = (currentTime * maxDayTime) / 50;
            Debug.Log(night);
            nightTime.color = new Color(nightTime.color.r, nightTime.color.g, nightTime.color.b, night);
        }

        coroutineDone = true;
        StopCoroutine(ChangeNightSky());
    }

    public void PassOut()
    {
        PassingOut();
        WakingUp();
    }

    public void PassingOut()
    {
        if (currentTime == maxDayTime)
        {
            if (!sleeping)
            {
                StartCoroutine(SleepAnim());
            }
            else
            {
                if (alpha < 1)
                {
                    alpha += Time.deltaTime * 0.5f;
                }
                else
                {
                    if (bed != null)
                        StartCoroutine(BlackScreen());
                    else if (bed == null)
                    {
                        loadNext.SceneChangeButton(SceneIndexBed);
                        Debug.Log("changed scene");
                    }
                }
                blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
            }            
        }
    }

    public void WakingUp()
    {
        if(passedOut)
        {            
            if (alpha >= 0)
            {
                alpha -= Time.deltaTime * 0.3f;
            }
            else
                passedOut = false;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
            wakingUpText.color = new Color(wakingUpText.color.r, wakingUpText.color.g, wakingUpText.color.b, alpha);
        }       
    }

    private IEnumerator BlackScreen()
    {
        if (bed != null)
        {
            playerMovement.animator.SetBool("IsSleeping", false);
            playerMovement.enabled = true;
            player.transform.position = bed.transform.position;
            collector.dayTime = 0;
            currentTime = 0;
            yield return new WaitForSeconds(2);
            passedOut = true;
            sleeping = false;
            StopCoroutine(BlackScreen());
        }
    }

    public IEnumerator SleepAnim()
    {
        playerMovement.animator.SetBool("IsSleeping", true);
        playerMovement.enabled = false;
        yield return new WaitForSeconds(3);
        sleeping = true;
        StopCoroutine(SleepAnim());
    }
}
