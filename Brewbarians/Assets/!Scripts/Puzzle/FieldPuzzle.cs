using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPuzzle : MonoBehaviour
{
    public List<ClickFieldPuzzle> wrongFields = new List<ClickFieldPuzzle>();
    public List<ClickFieldPuzzle> rightFields = new List<ClickFieldPuzzle>();
    public bool doorOpen;
    public GameObject[] lights;
    public GameObject bridgeObj;
    public Sprite newBridge;
    private bool allRight()
    {
        for (int i = 0; i < rightFields.Count; i++)
        {
            if (!rightFields[i].clicked)
                return false;
        }
        return true;
    }
    private bool allWrong()
    {
        for (int i = 0; i < wrongFields.Count; i++)
        {
            if (wrongFields[i].clicked)
                return false;
        }
        return true;
    }

    private void Update()
    {
        if(allWrong() && allRight())
        {
            doorOpen = true;
        }

        if(doorOpen)
        {
            //Sound Abspielen
            //Tür freischalten
            bridgeObj.GetComponent<SpriteRenderer>().sprite = newBridge;
            bridgeObj.GetComponent<BoxCollider2D>().enabled = false;

            foreach(GameObject light in lights)
            {
                light.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(false);
            }
        }
    }
}
