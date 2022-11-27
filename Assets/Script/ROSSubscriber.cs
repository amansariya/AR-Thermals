using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using ThermalCameraMsg = RosMessageTypes.ThermalCam.ThermalCameraMsgMsg;// as ThermalCameraMsg;

public class ROSSubscriber : MonoBehaviour
{
    Texture2D texture;
    ThermalCameraMsg lastMsg;
    uint lastSeqProcessed;

    [SerializeField] private Renderer planeRenderer;

    private readonly int width = ThermalCameraMsg.WIDTH;
    private readonly int height = ThermalCameraMsg.HEIGHT;

    // TODO (avinash) Change these magic numbers
    private readonly float maxTemp = 40.0f;
    private readonly float minTemp = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(height, width);

        planeRenderer.material.mainTexture = texture;
        ROSConnection.GetOrCreateInstance().Subscribe<ThermalCameraMsg>("thermals", Callback);
        lastSeqProcessed = uint.MaxValue;
        lastMsg = null;
    }
    void Callback(ThermalCameraMsg thermalMsg)
    {
        lastMsg = thermalMsg;
    }
    // Update is called once per frame
    void Update()
    {

        if (lastMsg != null && lastMsg.header.seq != lastSeqProcessed)
        {
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