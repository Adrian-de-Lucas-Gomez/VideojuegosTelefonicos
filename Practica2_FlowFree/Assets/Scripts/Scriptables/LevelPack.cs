using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    [CreateAssetMenu(fileName = "Pack",
        menuName = "Flow/Pack", order = 2)]
    public class LevelPack : ScriptableObject
    {
        //Nombre del pack de niveles
        public string packName;
        //Fichero con el conjunto de niveles
        public TextAsset levelsString;
        //Colores del tema
        public ColorSkin skin;
        //Texto de los grupos de niveles
        public string[] texts;
        //Colores de los grupos de niveles
        public Color[] colors;
    }
}
