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
        [SerializeField] Text nLevels;

        private LevelPack pack; //El scriptable

        void Start()
        {
#if UNITY_EDITOR
            if (levelPackTitle == null || nLevels == null)
            {
                Debug.LogError("CategoryCard: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
        }

        private void Configure()
        {
            if (pack == null) return;

            levelPackTitle.text = pack.packName;
            nLevels.text = "jeje/juju"; //TODO:
        }

        public void ConfigureLevelPack(LevelPack pack)
        {
            this.pack = pack;
            Configure();
        }

        public void OnClick()
        {
            Debug.Log("CLICK");
            levelPackTitle.color = Color.white;
        }
    }
}
