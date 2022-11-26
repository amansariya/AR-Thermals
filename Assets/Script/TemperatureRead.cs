using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.AccessControl;

public class TemperatureRead : MonoBehaviour
{
    int valueLeft = 25;
    int valueRight = 70;

    [SerializeField]
    private TextMeshPro tempValueLeft;

    [SerializeField]
    private TextMeshPro tempValueRight;

    [SerializeField]
    private GameObject colorIndicatorSphereLeft;

    [SerializeField]
    private GameObject colorIndicatorSphereRight;

    private Color newSphereColor;

    private Renderer sphereRendererLeft;
    private Renderer sphereRendererRight;

    // Start is called before the first frame update
    void Start()
    {
        sphereRendererLeft = colorIndicatorSphereLeft.GetComponent<Renderer>();
        sphereRendererRight = colorIndicatorSphereRight.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        tempValueLeft.text = valueLeft.ToString();
        tempValueRight.text = valueRight.ToString();
        SetColor(valueLeft, sphereRendererLeft);
        SetColor(valueRight, sphereRendererRight);
    }  

    void SetColor(int val, Renderer sphereRenderer)
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
