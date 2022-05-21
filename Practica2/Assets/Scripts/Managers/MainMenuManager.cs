using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    /// <summary>
    /// Gestiona los menus del juego
    /// </summary>

    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] Transform categoriesParent;
        [SerializeField] CategoryCard categoryCardPrefab;
        [SerializeField] LevelPackCard levelPackCardPrefab;
        [SerializeField] GameObject titleParent;

        private void LoadMainMenu()
        {
            List<Categories> categories = GameManager.GetInstance().GetCategories();
            //Crear las categorias de niveles
            for (int i = 0; i < categories.Count; i++)
            {
                CategoryCard card = Instantiate(categoryCardPrefab, categoriesParent);
                card.ConfigureCategory(categories[i]);

                //Crear los packs de niveles dentro de cada categoria
                List<LevelPack> packs = categories[i].packs;
                for (int j = 0; j < packs.Count; ++j)
                {
                    LevelPackCard pack = Instantiate(levelPackCardPrefab, categoriesParent);
                    pack.ConfigureLevelPack(packs[j], categories[i]);
                }
            }

            Color[] colors = GameManager.GetInstance().GetTheme();
            int numLetters = titleParent.transform.childCount;

            for (int i = 0; i < numLetters; i++)
            {
                Color aux = colors[i];
                aux.a = 1.0f;
                titleParent.transform.GetChild(i).GetComponent<Text>().color = aux;
            }
        }

        void Start()
        {
#if UNITY_EDITOR
            if (categoriesParent == null || categoryCardPrefab == null || levelPackCardPrefab == null || titleParent == null)
            {
                Debug.LogError("MenuManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
            LoadMainMenu();
        }
    }
}