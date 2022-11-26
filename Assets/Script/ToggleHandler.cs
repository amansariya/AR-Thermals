using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHandler : MonoBehaviour
{   
    public void ToggleDisplay(GameObject toggleObj)
    {
        if (toggleObj.activeSelf == true)
        {
            toggleObj.SetActive(false);
        }
        else
        {
            toggleObj.SetActive(true);
        }
    }
}
