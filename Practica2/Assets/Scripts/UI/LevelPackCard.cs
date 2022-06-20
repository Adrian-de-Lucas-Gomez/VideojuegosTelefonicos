using System;
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
        [SerializeField] Button packButton;
        [SerializeField] Button directLevelButton;

        private int nLevels;
        private LevelPack pack; //El scriptable
        private Categories category;


        void Start()
        {
#if UNITY_EDITOR
            if (levelPackTitle == null || nLevelsText == null || packButton == null)
            {
                Debug.LogError("LevelPackCard: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
        }

        private void Configure(int levelsCompleted)
        {
            if (pack == null) return;

            levelPackTitle.text = pack.title;

            levelPackTitle.color = Color.white;
            ColorBlock colors = packButton.colors;
            colors.normalColor = category.color;
            colors.highlightedColor = category.color;
            colors.pressedColor = Color.white;
            colors.selectedColor = Color.white;
            packButton.colors = colors;

            nLevels = pack.levelsFile.ToString().Split('\n').Length - 1;
            nLevelsText.text = levelsCompleted.ToString() + "/" + nLevels.ToString();
        }

        public void ConfigureLevelPack(LevelPack pack, Categories category, int levelsCompleted)
        {
            this.pack = pack;
            this.category = category;
            Configure(levelsCompleted);
            directLevelButton.onClick.AddListener(NextUnperfectLevel);

            if (GameManager.GetInstance().GetNextLevelNotPerfect() == -1) //Si no hay niveles para saltar
            {
                directLevelButton.interactable = false;
            }
        }

        private void NextUnperfectLevel()
        {
            GameManager.GetInstance().NextLevelNotPerfectMenu(category, pack);
        }

        public void OnClick()
        {
            GameManager.GetInstance().LoadLevelMenu(pack, category);
        }
    }
}
