using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    [CreateAssetMenu(fileName = "Category", 
        menuName = "Flow/Category", order = 1)]
    public class Categories : ScriptableObject
    {
        //Nombre de la categoria
        public string title;
        //Packs de niveles
        public List<LevelPack> packs;
        //Color en el menú
        public Color color;

        void OnEnable()
        {
#if UNITY_EDITOR
            if (title == null || packs.Count == 0 || color == null)
            {
                Debug.LogError("Category: faltan variables por asignar en el ScriptableObject.");
                return;
            }
#endif
        }
    }
}
