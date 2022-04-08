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
        public string title;
        //Fichero con el conjunto de niveles
        public TextAsset levelsFile;
        //Titulos de los grupos de niveles
        public string[] groupTitles;
        //Colores de los grupos de niveles
        public Color[] colors;

        void OnEnable()
        {
#if UNITY_EDITOR
            if (title == null || levelsFile == null)
            {
                Debug.LogError("LevelPack: faltan variables por asignar en el ScriptableObject.");
                return;
            }
#endif
        }
    }
}
