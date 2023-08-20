using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ToggleControl : MonoBehaviour
{
    public Toggle otherToggle;
    private Toggle thisToggle;
    public bool fullScreen;

    public void Start()
    {
        thisToggle = GetComponent<Toggle>();
    }

    public void Update()
    {
        thisToggle.onValueChanged.AddListener(delegate { OnToggleExec(); });
    }

    private void OnToggleExec()
    {
        if(thisToggle.isOn)
        {
            otherToggle.isOn = false;

            if(fullScreen)
            {
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }
        }
    }
}
