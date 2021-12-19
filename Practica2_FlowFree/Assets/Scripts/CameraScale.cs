using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour
{
    [SerializeField] float sceneWidth = 10;
    [SerializeField] GameObject sceneViewport;
    [SerializeField] Camera mainCamera;

    void Start()
    {

    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    void LateUpdate()
    {
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        mainCamera.orthographicSize = desiredHalfHeight;
    }
}
