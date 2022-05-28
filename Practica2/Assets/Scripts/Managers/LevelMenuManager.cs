using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    public class LevelMenuManager : MonoBehaviour
    {
        //const int LEVELS_PER_PAGE = 30;

        [SerializeField] Transform levelPageParent;
        [SerializeField] Text levelsMenuTitle;
        [SerializeField] LevelPage levelPagePrefab;
        [SerializeField] LevelButton levelButtonPrefab;

        private Categories currentCategory = null;
        private LevelPack currentLevelPack = null;
        private Transform levelButtonParent;

        private void LoadLevelMenu()
        {
            GameManager gMng = GameManager.GetInstance();
            currentCategory = gMng.GetSelectedCategory();
            currentLevelPack = gMng.GetSelectedPack();

            //Configurar el menu de niveles
            levelsMenuTitle.text = currentLevelPack.title;
            levelsMenuTitle.color = currentCategory.color;

            //Crear las paginas de niveles
            string[] levelsStrings = currentLevelPack.levelsFile.ToString().Split('\n');
            int nLevels = levelsStrings.Length - 1;
            int nPages = nLevels / GameManager.LEVELS_PER_PAGE;

            //ProgressData data = GameManager.GetInstance().GetData();

            for (int i = 0; i < nPages; i++)
            {
                LevelPage page = Instantiate(levelPagePrefab, levelPageParent);
                page.ConfigureLevelPage(i, currentLevelPack);

                //Crear LEVELS_PER_PAGE botones
                for (int j = 0; j < GameManager.LEVELS_PER_PAGE; ++j)
                {
                    levelButtonParent = page.GetButtonsParent();
                    LevelButton button = Instantiate(levelButtonPrefab, levelButtonParent);


                    bool locked = false;
                    if (gMng.GetProgressInPack().levels[j + GameManager.LEVELS_PER_PAGE * i].locked)
                    {
                        locked = true;
                    }
                    //Numero real del nivel
                    int levelIndex = j + GameManager.LEVELS_PER_PAGE * i + 1;
                    //Numero del nivel en la p�gina
                    int buttonNumber = levelIndex % GameManager.LEVELS_PER_PAGE;
                    if (buttonNumber == 0) buttonNumber = GameManager.LEVELS_PER_PAGE;

                    string levelButtonString = levelsStrings[j + GameManager.LEVELS_PER_PAGE * i];

                    button.ConfigureLevelButton(levelIndex, buttonNumber, currentLevelPack.colors[i], levelButtonString, locked, gMng.GetProgressInPack().levels[j + GameManager.LEVELS_PER_PAGE * i].completed);
                }
            }
        }

        void Start()
        {
#if UNITY_EDITOR
            if (levelPageParent == null || levelsMenuTitle == null || levelPagePrefab == null || levelButtonPrefab == null)
            {
                Debug.LogError("MenuManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
            LoadLevelMenu();
        }
    }
}
