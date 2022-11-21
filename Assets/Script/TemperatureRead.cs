using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.AccessControl;

public class TemperatureRead : MonoBehaviour
{
    int value1 = 25;

    [SerializeField]
    private TextMeshPro tempValue;

    [SerializeField]
    private GameObject colorIndicatorSphere;

    private Color newSphereColor;

    private Renderer sphereRenderer;

    [SerializeField]
    private GameObject test;

    private Renderer testRenderer;

    // Start is called before the first frame update
    void Start()
    {
        sphereRenderer = colorIndicatorSphere.GetComponent<Renderer>();
        testRenderer = test.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        tempValue.text = value1.ToString();
        testRenderer.material.color = Color.green;
        //SetColor1(value1);
    }  

    void SetColor1(int val)
    {
        if (val <= 30)
        {
            //newSphereColor = new Color();
            sphereRenderer.material.color = Color.cyan;
            //sphereRenderer.material.SetColor("_Color", newSphereColor);
        }
        else if (val > 30 && val <= 70)
        {
            //newSphereColor = new Color();
            sphereRenderer.material.color = Color.yellow;
            //sphereRenderer.material.SetColor("_Color", newSphereColor);
        }
        else if (val > 70)
        {
            //newSphereColor = new Color();
            sphereRenderer.material.color = Color.red;
            //sphereRenderer.material.SetColor("_Color", newSphereColor);
        }
    }
}
