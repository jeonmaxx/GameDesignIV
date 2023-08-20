using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsCollector : MonoBehaviour
{
    public float addedFarmPoints;
    public float addedBrewPoints;
    public float dayTime;
    public float secondsTilPoints = 5;

    public void Start()
    {
        StartCoroutine(WaitForAdding());
    }

    public void AddFarmPoints()
    {
        addedFarmPoints++;
    }

    public void AddBrewPoints()
    {
        addedBrewPoints++;
    }

    public void AddDayTime()
    {
        dayTime++;
    }

    public IEnumerator WaitForAdding()
    {
        yield return new WaitForSeconds(secondsTilPoints);
        AddBrewPoints();
        AddFarmPoints();
        AddDayTime();
        StopCoroutine(WaitForAdding());
    }
}
