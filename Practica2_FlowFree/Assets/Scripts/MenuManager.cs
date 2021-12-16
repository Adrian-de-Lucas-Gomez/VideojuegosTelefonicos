using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    /// <summary>
    /// Gestiona los menus del juego
    /// </summary>

    public class MenuManager : MonoBehaviour
    {
        const int LEVELS_PER_PAGE = 30;

        public enum Menu
        {
            MAIN,
            LEVELS
        }

        //GameMenus
        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject levelsMenu;

        //Para MainMenu
        [SerializeField] Categories[] categories; //Total de categorias de niveles en el juego
        [SerializeField] Transform categoriesParent;
        //Para LevelsMenu
        [SerializeField] Transform levelPageParent;
        private Transform levelButtonParent;
        [SerializeField] Text levelsMenuTitle;

        //Prefabs
        [SerializeField] CategoryCard categoryCardPrefab;
        [SerializeField] LevelPackCard levelPackCardPrefab;
        [SerializeField] LevelPage levelPagePrefab;
        [SerializeField] LevelButton levelButtonPrefab;

        private Menu currentMenu = Menu.MAIN;
        private Categories currentCategory = null;
        private LevelPack currentlevelPack = null;


        private void LoadMainMenu()
        {
            mainMenu.SetActive(true);
            levelsMenu.SetActive(false);

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
        }

        private void LoadLevelsMenu()
        {
            mainMenu.SetActive(false);
            levelsMenu.SetActive(true);

            //Configurar el menu de niveles
            levelsMenuTitle.text = currentlevelPack.title;
            levelsMenuTitle.color = currentCategory.color;

            //Crear las paginas de niveles
            string[] levelsStrings = currentlevelPack.levelsFile.ToString().Split('\n');
            int nLevels = levelsStrings.Length - 1;
            int nPages = nLevels / LEVELS_PER_PAGE;

            for (int i = 0; i < nPages; i++)
            {
                LevelPage page = Instantiate(levelPagePrefab, levelPageParent);
                page.ConfigureLevelPage(i, currentlevelPack);

                //Crear LEVELS_PER_PAGE botones
                for (int j = 0; j < LEVELS_PER_PAGE; ++j)
                {
                    levelButtonParent = page.GetButtonsParent();
                    LevelButton button = Instantiate(levelButtonPrefab, levelButtonParent);

                    int levelIndex = j + LEVELS_PER_PAGE * i + 1;
                    string levelButtonString = levelsStrings[j + LEVELS_PER_PAGE * i];
                    button.ConfigureLevelButton(levelIndex, currentlevelPack.colors[i], levelButtonString);
                }
            }
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        void Start()
        {
#if UNITY_EDITOR
            if (categories.Length == 0 || categoriesParent == null || categoryCardPrefab == null ||
                levelPackCardPrefab == null || levelPagePrefab == null || levelPageParent == null ||
                levelsMenuTitle == null)
            {
                Debug.LogError("MenuManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
            if(currentMenu == Menu.MAIN)
            {
                LoadMainMenu();
            }
        }

        public void OnChooseLevelPack(LevelPack pack, Categories packCategory)
        {
            currentlevelPack = pack;
            currentCategory = packCategory;
            currentMenu = Menu.LEVELS;

            LoadLevelsMenu();
        }

        public void OnExitMainMenu()
        {
            Application.Quit();
        }

        public void OnExitLevelMenu()
        {
            //TODO: ESTA MAL
            // -El LevelPack del que se quiere salir para actualizar si has completado niveles
            // -Borrar todo lo instanciado (level pages) en LEVELS 

            for (int i = levelPageParent.childCount - 1; i >= 0; ++i)
            {
                GameObject child = levelPageParent.GetChild(i).gameObject;
                Destroy(child);
            }

            currentlevelPack = null;
            currentCategory = null;
            currentMenu = Menu.MAIN;

            mainMenu.SetActive(true);
            levelsMenu.SetActive(false);
        }
    }
}