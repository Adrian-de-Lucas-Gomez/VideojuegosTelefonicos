using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace flow
{
    /// <summary>
    /// Transicion de escenas
    /// Gestiona la serializacion/progreso
    /// Es un prefab
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] int categoryIndex = 0;
        [SerializeField] int packIndex = 0;
        [SerializeField] int levelIndex = 0; //TODO
        [SerializeField] Categories[] categories;

        public Categories selectedCategory { get; set; }
        public LevelPack selectedPack { get; set; }
        public ColorSkin skin;

        private static GameManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                ////Pillar info del gameManager de esta escena
                //GetGMInfo(levelManager, categories);
                //Destroy(this);
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void Start()
        {
#if UNITY_EDITOR
            if (skin == null || categories == null)
            {
                Debug.LogError("GameManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //private void GetGMInfo(LevelManager levelManager, Categories[] categories)
        //{
        //    instance.categories = categories;
        //    instance.levelManager = levelManager;
        //}

        public Categories[] GetCategories()
        {
            return categories;
        }

        public LevelPack GetSelectedPack()
        {
            return categories[categoryIndex].packs[packIndex];
        }

        public static GameManager GetInstance()
        {
            return instance;
        }

        public Color[] GetTheme()
        {
            return skin.colors;
        }
    }
}