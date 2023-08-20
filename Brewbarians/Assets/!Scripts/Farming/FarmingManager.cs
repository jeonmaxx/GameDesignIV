using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Plants
{
    public Seed Seed;
    public PlantStates PlantState;
    public int GrowPoints;

    public Plants(Seed seed, PlantStates plantState, int growPoints)
    {
        Seed = seed;
        PlantState = plantState;
        GrowPoints = growPoints;
    }
}

[Serializable]
public class Fields
{
    public FieldStates FieldState;
    public int HoedPoints;
    public int WetPoints;

    public Fields(FieldStates fieldState, int hoedPoints, int wetPoints)
    {
        FieldState = fieldState;
        HoedPoints = hoedPoints;
        WetPoints = wetPoints;
    }
}

public class FarmingManager : MonoBehaviour
{
    public GameObject[] fieldObjs;
    public List<Planting> planting;
    public List<Growing> growing;
    public List<Plants> plants;
    public List<Fields> fields;

    public FarmSign[] farmSigns;
    public List<Item> signSeed;

    public PointsCollector collector;

    public void Awake()
    {
        if(fieldObjs.Length > 0)
        {
            planting = new List<Planting>();
            growing = new List<Growing>();
            foreach (GameObject field in fieldObjs)
            {
                planting.Add(field.GetComponent<Planting>());
                growing.Add(field.GetComponent<Growing>());
            }
        }
    }

    public void Start()
    {
        if(fieldObjs.Length > 0)
        {
            GiveFarmPoints();
        }
    }

    public void AddToList()
    {
        plants.Clear();
        fields.Clear();
        signSeed.Clear();
        if (fieldObjs.Length > 0)
        {
            for (int i = 0; i < fieldObjs.Length; i++)
            {
                if (planting[i].seed != null)
                    plants.Add(new Plants(planting[i].seed, planting[i].curPlantState, growing[i].currentGrowPoints));
                else
                    plants.Add(null);


                fields.Add(new Fields(planting[i].curFieldState, growing[i].currentHoedPoints, growing[i].currentWetPoints));
            }
        }

        if(farmSigns.Length > 0)
        {
            for(int i = 0; i < farmSigns.Length; i++)
            {
                signSeed.Add(farmSigns[i].signSeed);
            }
        }

    }

    public void UpdateFields()
    {
        string fieldPath = Application.persistentDataPath + "/" + "fields.json";
        if (fieldObjs.Length > 0 && File.Exists(fieldPath) && new FileInfo(fieldPath).Length > 0)
        {
            
            foreach (GameObject field in fieldObjs)
            {
                for(int i = 0; i < field.transform.childCount; i++)
                {
                    Destroy(field.transform.GetChild(i).gameObject);
                }
            }
   
            string plantsPath = Application.persistentDataPath + "/" + "plants.json";
            for (int i = 0; i < planting.Count; i++)
            {
                //planting[i].seed = plants[i].Seed;
                //planting[i].curPlantState = plants[i].PlantState;
                //growing[i].currentGrowPoints = plants[i].GrowPoints;
                //planting[i].AddSavedSeed();
                if (File.Exists(plantsPath) && new FileInfo(plantsPath).Length > 0)
                {
                    planting[i].GivenPlant(plants[i].Seed, plants[i].PlantState, plants[i].GrowPoints);
                }
                planting[i].GivenField(fields[i].FieldState, fields[i].HoedPoints, fields[i].WetPoints);
                Debug.Log("updated fields!");
            }
        }


        if (farmSigns.Length > 0)
        {
            string signPath = Application.persistentDataPath + "/" + "signSeeds.json";
            if (File.Exists(signPath) && new FileInfo(signPath).Length > 0)
            {
                for (int i = 0; i < farmSigns.Length; i++)
                {
                    farmSigns[i].signSeed = signSeed[i];
                }
            }
        }
    }

    public void GiveFarmPoints()
    {
        for(int i = 0; i < fieldObjs.Length; i++)
        {
            int tmpPoints = (int)collector.addedFarmPoints;

            if (planting[i].curPlantState != PlantStates.None
            && growing[i].currentGrowPoints < growing[i].phase3Points
            && planting[i].curFieldState == FieldStates.Wet)
            {
                if (tmpPoints + growing[i].currentWetPoints <= growing[i].wetPoints)
                {
                    growing[i].currentGrowPoints += tmpPoints;
                }
                else
                {
                    growing[i].currentGrowPoints += (growing[i].wetPoints - growing[i].currentWetPoints);
                }
            }

            if (planting[i].curFieldState == FieldStates.Hoed
                && planting[i].curPlantState == PlantStates.None)
            {
                growing[i].currentHoedPoints += tmpPoints;
            }

            if (planting[i].curFieldState == FieldStates.Wet
                && growing[i].currentWetPoints < growing[i].wetPoints)
            {
                growing[i].currentWetPoints += tmpPoints;
            }
        }

        collector.addedFarmPoints = 0;
    }
}
