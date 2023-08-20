using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeedWheelItem : MonoBehaviour, IPointerClickHandler
{
    private GameObject parent;

    private SeedWheelManager manager;


    public void Start()
    {
        parent = transform.parent.gameObject;
        manager = parent.GetComponent<SeedWheelManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.childCount != 0)
        {
            manager.seedObj = transform.GetChild(0).gameObject;
        }
    }
}
