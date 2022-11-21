using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugingGaze : MonoBehaviour
{
    public TextMeshPro txt;

    private void Update()
    {
        try
        {
            txt.text = CoreServices.InputSystem.EyeGazeProvider.GazeCursor.Position.ToString();
        }catch(Exception e)
        {
            txt.text = e.ToString();
        }
    }
}
