using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantState : MonoBehaviour
{
    [HideInInspector] public string[] plantStates = {"noPlant", "Phase01", "Phase02", "Phase03"};
    public string currentPlantState;

    public void Start()
    {
        currentPlantState = plantStates[0];
    }
}
