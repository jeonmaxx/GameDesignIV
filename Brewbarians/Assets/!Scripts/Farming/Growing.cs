using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Growing : MonoBehaviour
{
    [Header("Growing Points")]
    public int phase2Points = 5;
    public int phase3Points = 10;
    public int currentGrowPoints;

    [Header("Field Points")]
    public int hoedPoints = 5;
    [ShowOnly] 
    public int currentHoedPoints;
    public int wetPoints = 5;
    [ShowOnly]
    public int currentWetPoints;

    private Planting planting;
    public PointsCollector collector;

    public void Start()
    {
        planting = GetComponent<Planting>();
    }

    public void Update()
    {
        CheckPlantState();
        CheckFieldStates();
    }   

    public void CheckPlantState()
    {
        if (planting.curPlantState != PlantStates.None)
        {
            if(currentGrowPoints < phase2Points)
            {
                planting.curPlantState = PlantStates.Phase01;
            }
            else if(currentGrowPoints >= phase2Points && currentGrowPoints < phase3Points)
            {
                planting.curPlantState = PlantStates.Phase02;
            }
            else if(currentGrowPoints >= phase3Points)
            {
                planting.curPlantState = PlantStates.Phase03;
            }
        }

        if (planting.curPlantState == PlantStates.None)
            currentGrowPoints = 0;
    }

    public void CheckFieldStates()
    {
        if(currentHoedPoints >= hoedPoints)
        {
            currentHoedPoints = 0;
            planting.curFieldState = FieldStates.None;
            Destroy(planting.hoed);
        }

        if(currentWetPoints >= wetPoints)
        {
            currentWetPoints = 0;
            planting.curFieldState = FieldStates.Hoed;
            Destroy(planting.wet);
        }
    }
}
