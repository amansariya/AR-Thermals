using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiniApp : MonoBehaviour
{
    protected virtual void Start() { }

    private void OnEnable()
    {
        PlaceInFrontOfUser();
    }

    private void PlaceInFrontOfUser()
    {
        var cameraTransform = Camera.main.transform;
        var forwardXZ = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
        forwardXZ.Normalize();
        transform.position = cameraTransform.position + forwardXZ;
    }
}

