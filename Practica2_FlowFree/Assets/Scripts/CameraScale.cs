using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour
{
    [SerializeField] float sceneWidth = 10;
    [SerializeField] RectTransform sceneViewport;
    [SerializeField] Camera mainCamera;

    void Start()
    {



        FitViewport();
    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    void LateUpdate()
    {

        FitViewport();
        //float unitsPerPixel = sceneWidth / Screen.width;
        //float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        //mainCamera.orthographicSize = desiredHalfHeight;


        //Canvas.ForceUpdateCanvases();


    }

    private void FitViewport()
    {
        if (sceneViewport == null) return;

        //Centrar la camara al viewport de la escena
        mainCamera.transform.position = sceneViewport.rect.center;



        //float wRatio = sceneViewport.rect.width / sceneViewport.rect.width;
        //float hRatio = sceneViewport.rect.height / sceneViewport.rect.height;

        //mainCamera.rect = new Rect(1.0f - wRatio, 0.0f, wRatio, hRatio);
    }
}
