using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BushData
{
    public BushScene Scene;
    public List<bool> Empty;
    public BushData(BushScene scene, List<bool> empty) 
    {
        Scene = scene;
        Empty = empty;
    }
}

public enum BushScene {Village, Harbor, Farmland}
public class SaveBushes : MonoBehaviour
{
    //Notiz:
    //Saving mit Enum machen in welcher Szene man sich befindet (3 Szenen)
    //saving 3 Listen (wo je die Büsche mit ihren States drin sind)
    public BushData bushData;
    public List<HarvestBushes> bushes;

    //public void Update()
    //{
    //    if (updated)
    //    {
    //        CollectBushes();
    //    }
    //}

    public void CollectBushes()
    {
        bushData.Empty = new List<bool>();
        foreach (var bush in bushes)
        {
            bushData.Empty.Add(bush.emptyBool);
        }
    }

}
