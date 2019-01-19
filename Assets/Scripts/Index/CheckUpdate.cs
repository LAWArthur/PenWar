using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

public class CheckUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        if (ExternalFunctionality.MessageBox(IntPtr.Zero, "Are you sure to download update? ", "Confirm", 1) == 1)
        {
            Application.OpenURL(@"https://raw.githubusercontent.com/LAWArthur/PenWar/master/Releases/setup.exe");
        }
    }
}
