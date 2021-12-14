using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    /// <summary>
    /// Crea las categorias de niveles en el canvas
    /// </summary>

    public class MenuManager : MonoBehaviour
    {
        [SerializeField] Categories[] categories;
        [SerializeField] Transform categoriesParent;
        [SerializeField] CategoryCard categoryCardPrefab;
        [SerializeField] LevelPackCard levelPackCardPrefab;

        void Start()
        {
#if UNITY_EDITOR
            if (categories.Length == 0 || categoriesParent == null || categoryCardPrefab == null ||
                levelPackCardPrefab == null)
            {
                Debug.LogError("MenuManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
            //Crear las categorias de niveles
            for (int i = 0; i < categories.Length; i++)
            {
                CategoryCard card = Instantiate(categoryCardPrefab, categoriesParent);
                card.ConfigureCategory(categories[i]);

                LevelPack[] packs = categories[i].packs;
                for (int j = 0; j < packs.Length; ++j)
                {
                    LevelPackCard pack = Instantiate(levelPackCardPrefab, categoriesParent);
                    pack.ConfigureLevelPack(packs[j], categories[i]);
                }
            }
        }

        public void OnExit() //Evento de salida al pulsar un boton
        {
            Application.Quit();
        }
    }
}