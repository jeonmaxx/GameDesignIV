using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMask : PlayerNear
{
    void Update()
    {
        CalcDistance();
        if(isPlayerNear)
            transform.GetChild(0).gameObject.SetActive(false);
        else
            transform.GetChild(0).gameObject.SetActive(true);
    }
}
