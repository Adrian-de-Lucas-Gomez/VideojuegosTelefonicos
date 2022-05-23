using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    public class LevelMenuManager : MonoBehaviour
    {
        const int LEVELS_PER_PAGE = 30;

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
            int nPages = nLevels / LEVELS_PER_PAGE;

            //ProgressData data = GameManager.GetInstance().GetData();

            for (int i = 0; i < nPages; i++)
            {
                LevelPage page = Instantiate(levelPagePrefab, levelPageParent);
                page.ConfigureLevelPage(i, currentLevelPack);

                levelButtonParent = page.GetButtonsParent();
                LevelButton buttonOne = Instantiate(levelButtonPrefab, levelButtonParent);

                int levelIndexAUX = 0 + LEVELS_PER_PAGE * i + 1;
                string levelButtonStringAux = levelsStrings[0 + LEVELS_PER_PAGE * i];
                buttonOne.ConfigureLevelButton(levelIndexAUX, currentLevelPack.colors[i], levelButtonStringAux, false);

                //Crear LEVELS_PER_PAGE botones
                for (int j = 1; j < LEVELS_PER_PAGE; ++j)
                {
                    levelButtonParent = page.GetButtonsParent();
                    LevelButton button = Instantiate(levelButtonPrefab, levelButtonParent);

                    bool locked = false;
                    if (currentCategory.fullyUnlocked)
                    {
                        locked = true;
                    }

                    int levelIndex = j + LEVELS_PER_PAGE * i + 1;
                    string levelButtonString = levelsStrings[j + LEVELS_PER_PAGE * i];
                    button.ConfigureLevelButton(levelIndex, currentLevelPack.colors[i], levelButtonString, locked);
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
