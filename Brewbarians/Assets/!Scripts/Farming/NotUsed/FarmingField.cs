using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class FarmingField : MonoBehaviour, IPointerDownHandler
{
    [Header("Field Types")]
    [SerializeField] private GameObject HoedField;
    [SerializeField] private GameObject WetField;
    public Item shovelItem;
    public Item waterItem;
    public HandManager handManager;
    private FieldState fieldState;

    public void Start()
    {
        fieldState = GetComponent<FieldState>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ShovelField();
        WaterField();
    }

    public void ShovelField()
    {
        if(fieldState.currentFieldState == fieldState.fieldStates[0] && handManager.handItem == shovelItem )
        {
            Instantiate(HoedField, gameObject.transform.position, gameObject.transform.rotation, this.transform);
            fieldState.currentFieldState = fieldState.fieldStates[1];
        }
    }

    public void WaterField()
    {
        if (fieldState.currentFieldState == fieldState.fieldStates[1] && handManager.handItem == waterItem)
        {
            Instantiate(WetField, gameObject.transform.position, gameObject.transform.rotation, this.transform);
            fieldState.currentFieldState = fieldState.fieldStates[2];
        }
    }

}
