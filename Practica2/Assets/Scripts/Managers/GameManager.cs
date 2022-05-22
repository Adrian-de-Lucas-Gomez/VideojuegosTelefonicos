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
        [SerializeField] int levelIndex = 0;

        [SerializeField] List<Categories> categories;

        public ColorSkin skin;

        private string[] packStrings = null;


        private static GameManager instance;

        private int nHints = 0;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
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
            packStrings = GetSelectedPack().levelsFile.ToString().Split('\n');
        }

        public void CloseGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void LoadLevelMenu(LevelPack pack, Categories packCategory)
        {
            categoryIndex = categories.IndexOf(packCategory);
            packIndex = categories[categoryIndex].packs.IndexOf(pack);
            packStrings = pack.levelsFile.ToString().Split('\n');
            levelIndex = 0;

            SceneManager.LoadScene("LevelMenu");

            AdvertisingManager.GetInstance().ShowIntersticialAd();
        }

        public void LoadMainMenu()
        {
            //Al volver al menu principal se les da un valor por defecto
            categoryIndex = 0;
            packIndex = 0;
            levelIndex = 0;
            packStrings = categories[categoryIndex].packs[0].levelsFile.ToString().Split('\n');

            SceneManager.LoadScene("MainMenu");

            AdvertisingManager.GetInstance().ShowIntersticialAd();
        }

        //Si queremos borrar algo antes de q cierre
        //private void OnApplicationQuit()
        //{
            
        //}

        public int NextLevel()
        {
            int levelsInPack = packStrings.Length - 1;

            if (levelIndex + 1 < levelsInPack)
            {
                return ++levelIndex;
            }                
            return -1; //-1 es que no existe nivel siguiente
        }

        public int PrevLevel()
        {
            if (levelIndex - 1 >= 0)
            {
                return --levelIndex;
            }
            return -1; //-1 es que no existe nivel anterior
        }

        public void OnHintAdded()
        {
            //Añadir una pista
            nHints = nHints + 1;
            Debug.Log(nHints);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public static GameManager GetInstance()
        {
            return instance;
        }

        public List<Categories> GetCategories()
        {
            return categories;
        }

        public Categories GetSelectedCategory()
        {
            return categories[categoryIndex];
        }

        public LevelPack GetSelectedPack()
        {
            return categories[categoryIndex].packs[packIndex];
        }

        public int GetSelectedLevelId()
        {
            return levelIndex;
        }

        public string[] GetSelectedPackStrings()
        {
            return packStrings;
        }

        public Color[] GetTheme()
        {
            return skin.colors;
        }

        public string GetSelectedLevelString()
        {
            return packStrings[levelIndex];
        }
    }
}