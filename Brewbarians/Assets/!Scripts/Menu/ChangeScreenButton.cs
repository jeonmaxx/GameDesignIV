using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChangeScreenButton : MonoBehaviour
{
    private bool pauseMenu;
    public bool isWholeMenu;

    public void Start()
    {
        if(isWholeMenu)
            pauseMenu = false;
    }

    public void Update()
    {
        if(isWholeMenu)
        {
            if(pauseMenu)
                transform.localScale = Vector3.one;
            if(!pauseMenu) 
                transform.localScale = Vector3.zero;
        }
    }

    public void OnScreenButton(Transform toScreen)
    {
        transform.localScale = Vector3.zero;
        toScreen.localScale = Vector3.one;
    }

    public void OpenCloseScreen()
    {
        if(!pauseMenu)
        {
            pauseMenu = true;
        }
        else if (pauseMenu)
        {
            pauseMenu = false;
        }
    }
}
