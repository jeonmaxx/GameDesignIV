using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldState : MonoBehaviour
{
    [HideInInspector] public string[] fieldStates = { "default", "hoed", "wet" };
    public string currentFieldState;

    public void Start()
    {
        currentFieldState = fieldStates[0];
    }
}
