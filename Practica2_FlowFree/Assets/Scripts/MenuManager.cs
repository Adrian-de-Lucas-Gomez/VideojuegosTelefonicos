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
        [SerializeField] Text levelsMenuTitle;

        //Prefabs
        [SerializeField] CategoryCard categoryCardPrefab;
        [SerializeField] LevelPackCard levelPackCardPrefab;
        [SerializeField] LevelPage levelPagePrefab;
        [SerializeField] LevelButton levelButtonPrefab;

        private Menu currentMenu = Menu.MAIN;
        private Categories currentCategory = null;
        private LevelPack currentlevelPack = null;
        private Transform levelButtonParent;

        [SerializeField] Text[] colorLetters;
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

            Color[] colors = GameManager.GetInstance().GetTheme();

            for(int i=0; i < colorLetters.Length; i++)
            {
                Color aux = colors[i];
                aux.a = 1.0f;
                colorLetters[i].color = aux;
                //colorLetters[i].color.a = 1.0f;
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


            ProgressData data = GameManager.GetInstance().GetData();

            for (int i = 0; i < nPages; i++)
            {
                LevelPage page = Instantiate(levelPagePrefab, levelPageParent);
                page.ConfigureLevelPage(i, currentlevelPack);


                levelButtonParent = page.GetButtonsParent();
                LevelButton buttonOne = Instantiate(levelButtonPrefab, levelButtonParent);

                int levelIndexAUX = 0 + LEVELS_PER_PAGE * i + 1;
                string levelButtonStringAux = levelsStrings[0 + LEVELS_PER_PAGE * i];
                buttonOne.ConfigureLevelButton(levelIndexAUX, currentlevelPack.colors[i], levelButtonStringAux, false);

                //Crear LEVELS_PER_PAGE botones
                for (int j = 1; j < LEVELS_PER_PAGE; ++j)
                {
                    levelButtonParent = page.GetButtonsParent();
                    LevelButton button = Instantiate(levelButtonPrefab, levelButtonParent);

                    bool locked = false;
                    if(currentCategory.levelsLocked)
                    {
                        locked = true;
                    }

                    int levelIndex = j + LEVELS_PER_PAGE * i + 1;
                    string levelButtonString = levelsStrings[j + LEVELS_PER_PAGE * i];
                    button.ConfigureLevelButton(levelIndex, currentlevelPack.colors[i], levelButtonString, locked);
                }
            }
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        void Start()
        {
#if UNITY_EDITOR
            if (mainMenu == null || levelsMenu == null || categoriesParent == null ||
                categoryCardPrefab == null || levelPackCardPrefab == null || levelPagePrefab == null ||
                levelPageParent == null || levelsMenuTitle == null || levelButtonPrefab == null)
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

            GameManager.GetInstance().selectedCategory = currentCategory;
            GameManager.GetInstance().selectedPack = currentlevelPack;

            LoadLevelsMenu();
        }

        public void OnExitMainMenu()
        {
            Application.Quit();
        }

        public void OnExitLevelMenu()
        {
            levelPageParent.transform.localPosition = Vector3.zero;
            foreach (Transform child in levelPageParent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            currentlevelPack = null;
            currentCategory = null;
            currentMenu = Menu.MAIN;

            mainMenu.SetActive(true);
            levelsMenu.SetActive(false);
        }
    }
}