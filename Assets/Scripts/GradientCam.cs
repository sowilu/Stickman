using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientCam : MonoBehaviour
{
    public Gradient gradient;
    public Camera cam;

    void Start()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
    }

    void Update()
    {
        cam.backgroundColor = GradientColor();
    }

    Color GradientColor()
    {
        float t = (cam.orthographicSize - cam.nearClipPlane) / (cam.farClipPlane - cam.nearClipPlane);
        return gradient.Evaluate(t);
    }
}
