using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class PlayerInput
    {
        static public Vector3 GetPointerPosition()
        {
            Vector3 position;
#if UNITY_ANDROID
        position = Input.GetTouch(0).position;
#else //PC & UNITY_EDITOR
        position = Input.mousePosition;
#endif
            return Camera.main.ScreenToWorldPoint(position);
        }

        static public bool JustPressed()
        {
#if UNITY_ANDROID
        return Input.GetTouch(0).phase == TouchPhase.Began;
#else
        return Input.GetMouseButtonDown(0);
#endif
        }

        static public bool JustUnpressed()
        {
#if UNITY_ANDROID
        return Input.GetTouch(0).phase == TouchPhase.Ended;
#else   
        return Input.GetMouseButtonUp(0);
#endif
        }

        static public bool IsPressed()
        {
#if UNITY_ANDROID
        return Input.GetTouch(0).phase == TouchPhase.Moved;
#else 
        return Input.GetMouseButton(0);
#endif
        }
    }

}