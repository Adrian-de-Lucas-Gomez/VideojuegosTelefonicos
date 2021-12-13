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

        private LevelPack pack; //El scriptable
        private int nLevels;
        private Categories category;

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

            levelPackTitle.text = pack.packName;
            levelPackTitle.color = category.color;

            nLevels = pack.levelsString.ToString().Split('\n').Length - 1;
            nLevelsText.text = "0" + "/" + nLevels.ToString(); //TODO:
        }

        public void ConfigureLevelPack(LevelPack pack, Categories category)
        {
            this.pack = pack;
            this.category = category;
            Configure();
        }

        public void OnClick()
        {
            levelPackTitle.color = Color.white;
        }
    }
}
