using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class CameraFitBoard : MonoBehaviour
    {
        [SerializeField] Camera mainCamera;
        [SerializeField] RectTransform contentRect;
        [SerializeField] RectTransform sceneViewport;
        [SerializeField] BoardManager boardManager;

        void Start()
        {



            FitViewport();
        }

        // Adjust the camera's height so the desired scene width fits in view
        // even if the screen/window size changes dynamically.
        void LateUpdate()
        {

            FitViewport();



            //Canvas.ForceUpdateCanvases();


        }

        private void FitViewport()
        {
            if (sceneViewport == null) return;

            Camera mainCamera = Camera.main;
            Canvas.ForceUpdateCanvases();

            float wRatio = sceneViewport.rect.width / contentRect.rect.width;
            float hRatio = sceneViewport.rect.height / contentRect.rect.height;

            mainCamera.rect = new Rect(1.0f - wRatio, 0.0f, wRatio, hRatio);
        }
    }
}