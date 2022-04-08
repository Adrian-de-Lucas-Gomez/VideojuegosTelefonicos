using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    public class LevelPackCard : MonoBehaviour
    {
        //Para UI
        [SerializeField] Text levelPackTitle;
        [SerializeField] Text nLevelsText;

        private int nLevels;
        private LevelPack pack; //El scriptable
        private Categories category;
        //private MainMenuManager mainMenuMng;


        void Start()
        {
#if UNITY_EDITOR
            if (levelPackTitle == null || nLevelsText == null)
            {
                Debug.LogError("LevelPackCard: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
        }

        private void Configure()
        {
            if (pack == null) return;

            levelPackTitle.text = pack.title;
            levelPackTitle.color = category.color;

            nLevels = pack.levelsFile.ToString().Split('\n').Length - 1;
            nLevelsText.text = "0" + "/" + nLevels.ToString();
        }

        public void ConfigureLevelPack(MainMenuManager mainMenuMng, LevelPack pack, Categories category)
        {
            //this.mainMenuMng = mainMenuMng;
            this.pack = pack;
            this.category = category;
            Configure();
        }

        //public void OnClick()
        //{
        //    //levelPackTitle.color = Color.white;
        //    menuManager.OnChooseLevelPack(pack, category);
        //}
    }
}
