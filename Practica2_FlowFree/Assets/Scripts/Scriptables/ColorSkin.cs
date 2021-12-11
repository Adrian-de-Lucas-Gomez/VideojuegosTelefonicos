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
    }
}
