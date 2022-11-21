using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectedFeedback : MonoBehaviour
{
    public GameObject timeBasedFeedback;
    // Start is called before the first frame update
    public float timeAmount;

    [SerializeField] private ObjectDelayEntry[] objectDelayEntries;

    IEnumerator ToggleFeedback(float delay)
    {
        yield return new WaitForSeconds(delay);
        timeBasedFeedback.SetActive(true);

        yield return new WaitForSeconds(delay);
        timeBasedFeedback.SetActive(false);
    }

    public void StartToggleFeedback()
    {
        StartCoroutine(ToggleFeedback(timeAmount));
    }

    [Serializable]
    public class ObjectDelayEntry
    {
        public GameObject gameObject;
        public float delay;
    }
}
