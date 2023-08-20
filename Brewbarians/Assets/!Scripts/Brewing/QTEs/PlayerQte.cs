using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQte : MonoBehaviour
{
    [SerializeField] private GameObject yellowZoneObj;
    [SerializeField] private GameObject greenZoneObj;
    public bool greenZone;
    public bool yellowZone;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == yellowZoneObj)
            yellowZone = true;

        if (other.gameObject == greenZoneObj)
            greenZone = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == yellowZoneObj)
            yellowZone = false;

        if (other.gameObject == greenZoneObj)
            greenZone = false;
    }
}
