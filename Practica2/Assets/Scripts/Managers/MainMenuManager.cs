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

        //private Categories currentCategory = null;
        //private LevelPack currentlevelPack = null;

        private void LoadMainMenu()
        {
            Categories[] categories = GameManager.GetInstance().GetCategories();
            //Crear las categorias de niveles
            for (int i = 0; i < categories.Length; i++)
            {
                CategoryCard card = Instantiate(categoryCardPrefab, categoriesParent);
                card.ConfigureCategory(categories[i]);

                //Crear los packs de niveles dentro de cada categoria
                LevelPack[] packs = categories[i].packs;
                for (int j = 0; j < packs.Length; ++j)
                {
                    LevelPackCard pack = Instantiate(levelPackCardPrefab, categoriesParent);
                    pack.ConfigureLevelPack(this, packs[j], categories[i]);
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

        //public void OnChooseLevelPack(LevelPack pack, Categories packCategory)
        //{
        //    currentlevelPack = pack;
        //    currentCategory = packCategory;
        //    currentMenu = Menu.LEVELS;

        //    GameManager.GetInstance().selectedCategory = currentCategory;
        //    GameManager.GetInstance().selectedPack = currentlevelPack;

        //    LoadLevelsMenu();
        //}

        //public void OnExitMainMenu()
        //{
        //    Application.Quit();
        //}
    }
}