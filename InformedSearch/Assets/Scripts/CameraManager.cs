using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float zoomBy = 2.0f;

    public void ZoomIn()
    {
        transform.Translate(new Vector3(0, 0, zoomBy));
    }

    public void ZoomOut()
    {
        transform.Translate(new Vector3(0, 0, -zoomBy));
    }

}
