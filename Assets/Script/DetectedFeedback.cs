using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectedFeedback : MonoBehaviour
{
    public GameObject timeBasedFeedback;
    // Start is called before the first frame update
    public float timeAmount;

    //[SerializeField]
    //private ObjectDelayEntry[] objectDelayEntries;

    [SerializeField]
    public GameObject[] spikeElements;

    public float spikeTime;

    IEnumerator ToggleSpikes(GameObject[] gobj,float delay)
    {
        for (int i = 0; i < gobj.Length; i++)
        {
            gobj[i].SetActive(true);
        }
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < gobj.Length; i++)
        {
            gobj[i].SetActive(false);
        }
    }

    IEnumerator ToggleFeedback(float delay)
    {
        timeBasedFeedback.SetActive(true);
        yield return new WaitForSeconds(delay);
        timeBasedFeedback.SetActive(false);
    }

    public void StartToggleFeedback()
    {
        StartCoroutine(ToggleFeedback(timeAmount));
    }

    public void SpikeDisplay()
    {
        StartCoroutine(ToggleSpikes(spikeElements, spikeTime));
    }

    [Serializable]
    public class ObjectDelayEntry
    {
        public GameObject gameObject;
        public float delay;
    }
}
