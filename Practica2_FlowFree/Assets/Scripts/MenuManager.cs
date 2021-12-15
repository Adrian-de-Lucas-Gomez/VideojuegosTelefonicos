using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    /// <summary>
    /// Crea las categorias de niveles en el canvas
    /// </summary>

    public class MenuManager : MonoBehaviour
    {
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

        private Menu currentMenu = Menu.MAIN;
        private Categories currentCategory = null;
        private LevelPack currentlevelPack = null;


        private void LoadMainMenu()
        {
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
                    pack.ConfigureLevelPack(packs[j], categories[i]);
                }
            }
        }

        private void LoadLevelsMenu()
        {
            //Configurar el menu de niveles
            levelsMenuTitle.text = currentlevelPack.title;
            levelsMenuTitle.color = currentCategory.color;

            //Crear las paginas de niveles
            int nLevels = currentlevelPack.levelsFile.ToString().Split('\n').Length - 1;
            int nPages = nLevels / 30;

            for (int i = 0; i < nPages; i++)
            {
                LevelPage page = Instantiate(levelPagePrefab, levelPageParent);
                page.ConfigureLevelPage(i, currentlevelPack);
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
                mainMenu.SetActive(true);
                levelsMenu.SetActive(false);

                LoadMainMenu();
            }
        }

        public void OnExit() //Evento de salida al pulsar un boton
        {
            Application.Quit();
        }

        public void OnChooseLevelPack()
        {

        }
    }
}