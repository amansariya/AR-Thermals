using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.AccessControl;
using Unity.Robotics.ROSTCPConnector;
using ThermalCameraMsg = RosMessageTypes.ThermalCam.ThermalCameraMsgMsg;// as ThermalCameraMsg;
using System;

public class TemperatureRead : MonoBehaviour
{
    float valueLeft;
    float valueRight;

    Texture2D texture;
    ThermalCameraMsg lastMsg;
    //public float rightTemp, leftTemp;
    uint lastSeqProcessed;

    [SerializeField]
    private TextMeshPro tempValueLeft;

    [SerializeField]
    private TextMeshPro tempValueRight;

    [SerializeField]
    private GameObject colorIndicatorSphereLeft;

    [SerializeField]
    private GameObject colorIndicatorSphereRight;

    //private Color newSphereColor;

    private Renderer sphereRendererLeft;
    private Renderer sphereRendererRight;

    [SerializeField] private Renderer planeRenderer;

    private readonly int width = ThermalCameraMsg.WIDTH;
    private readonly int height = ThermalCameraMsg.HEIGHT;

    // TODO (avinash) Change these magic numbers
    private readonly float maxTemp = 35.0f;
    private readonly float minTemp = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        sphereRendererLeft = colorIndicatorSphereLeft.GetComponent<Renderer>();
        sphereRendererRight = colorIndicatorSphereRight.GetComponent<Renderer>();
        ROSConnection.GetOrCreateInstance().Subscribe<ThermalCameraMsg>("thermals", Callback);
        texture = new Texture2D(height, width);
        planeRenderer.material.mainTexture = texture;
        //ROSConnection.GetOrCreateInstance().Subscribe<ThermalCameraMsg>("thermals", Callback);
        lastSeqProcessed = uint.MaxValue;
        lastMsg = null;
        valueLeft = -100;
        valueRight = -100;
    }

    private void Callback(ThermalCameraMsg obj)
    {
        lastMsg = obj;
        /*valueLeft = leftTemp;
        valueRight = rightTemp;*/
    }

    void getThermalValues(float[] arr)
    {
        //Debug.Log("width : " + width.ToString());
        //Debug.Log("height : " + height.ToString());
        int THRESHOLD = 20;
        int numberLeft = 0;
        int numberRight = 0;
        float sumLeft = 0;
        float sumRight = 0;

        //Debug.Log("in the func");
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (arr[(width * i) + j] > THRESHOLD)
                {
                    if (j < width / 2)
                    {
                        numberRight++;
                        sumRight += arr[(width * i) + j];
                    }
                    else
                    {
                        numberLeft++;
                        sumLeft += arr[(width * i) + j];
                    }
                }
            }
        }
        //Debug.Log("done processing");

        float tempRightTemp = (float)sumRight / numberRight;
        float tempLeftTemp = (float)sumLeft / numberLeft;
        if (tempRightTemp != float.NaN)
            valueRight = tempRightTemp;
        if (tempLeftTemp != float.NaN)
            valueLeft = tempLeftTemp;
        //Debug.Log("sumRight : " + sumRight.ToString());
        //Debug.Log("numRight : " + numberRight.ToString());
        //Debug.Log("exiting");
    }

    void Update()
    {
        //Debug.Log("RightTemp = " + valueRight);
        //Debug.Log("LeftTemp = " + valueLeft);
        tempValueLeft.text = valueLeft.ToString();
        tempValueRight.text = valueRight.ToString();
        SetColor(valueLeft, sphereRendererLeft);
        SetColor(valueRight, sphereRendererRight);
        if (lastMsg != null && lastMsg.header.seq != lastSeqProcessed)
        {
            getThermalValues(lastMsg.thermal_image);
            lastSeqProcessed = lastMsg.header.seq;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var idx = i * width + j;
                    // var tempVal = (lastMsg.thermal_image[idx] - minTemp)/maxTemp;
                    // var color = Color.Lerp(Color.blue, Color.green, tempVal);
                    var color = GetColour(lastMsg.thermal_image[idx]);
                    texture.SetPixel(i, width - j - 1, color);
                }
            }

            texture.Apply();
        }
    }

    void SetColor(float val, Renderer sphereRenderer)
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

    Color GetColour(float v)
    {
        Color c = new Color(1.0f, 1.0f, 1.0f, 1.0f); // white
        float dv;

        if (v < minTemp)
            v = minTemp;
        if (v > maxTemp)
            v = maxTemp;
        dv = maxTemp - minTemp;

        if (v < (minTemp + 0.25 * dv))
        {
            c.r = 0;
            c.g = 4 * (v - minTemp) / dv;
        }
        else if (v < (minTemp + 0.5 * dv))
        {
            c.r = 0;
            c.b = 1 + 4 * (minTemp + 0.25f * dv - v) / dv;
        }
        else if (v < (minTemp + 0.75 * dv))
        {
            c.r = 4 * (v - minTemp - 0.5f * dv) / dv;
            c.b = 0;
        }
        else
        {
            c.g = 1 + 4 * (minTemp + 0.75f * dv - v) / dv;
            c.b = 0;
        }

        return (c);
    }
}
