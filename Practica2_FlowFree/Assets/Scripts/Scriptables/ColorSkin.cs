using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    [CreateAssetMenu(fileName = "ColorSkin",
        menuName = "Flow/ColorSkin", order = 3)]
    public class ColorSkin : ScriptableObject
    {
        //Colores del tema
        public Color[] colors;

        void OnEnable()
        {
#if UNITY_EDITOR
            if (colors.Length == 0)
            {
                Debug.LogError("ColorSkin: faltan variables por asignar en el ScriptableObject.");
                return;
            }
#endif
        }
    }
}
