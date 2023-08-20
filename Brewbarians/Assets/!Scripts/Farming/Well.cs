using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Well : PlayerNear, IPointerDownHandler
{
    public Item waterItem;
    public HandManager handManager;
    public ToolSoundManager toolSoundManager;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        CalcDistance();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RefillWater();
    }

    public void RefillWater()
    {
        if(isPlayerNear && handManager.handItem == waterItem)
        {
            audioSource.clip = toolSoundManager.refillSounds[Random.Range(0, toolSoundManager.refillSounds.Length)];
            audioSource.Play();
            waterItem.currentWater = waterItem.waterAmount;
        }
    }
}
