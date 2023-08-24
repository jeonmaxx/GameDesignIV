using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPuzzle : MonoBehaviour
{
    public List<ClickFieldPuzzle> wrongFields = new List<ClickFieldPuzzle>();
    public List<ClickFieldPuzzle> rightFields = new List<ClickFieldPuzzle>();
    public bool doorOpen;
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
            //Sound Abspielen
        }
    }
}
