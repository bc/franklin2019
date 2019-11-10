using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AdjustTaskHeight : MonoBehaviour
{
    private Camera camera1;

    private void Start()
    {
        camera1 = Camera.main;
        MatchTaskToFace();
    }

    private void MatchTaskToFace()
    {
        transform.position = new Vector3(0f, camera1.transform.position.y - 0.012f, 0f);
    }

    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.LTouch))
        {
            MatchTaskToFace();
        }

        var xy = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            transform.position += Vector3.up * xy.y * 0.05f;
        }
}
