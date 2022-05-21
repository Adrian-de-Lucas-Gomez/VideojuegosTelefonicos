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
        [SerializeField] List<Categories> categories;

        //no se si deberiamos tener esto aqui tb?
        // packStrings = currentPack.levels.text.Split('\n');

        public ColorSkin skin;

        private static GameManager instance;

        //void Awake()
        //{
        //    if (instance == null)
        //    {
        //        instance = this;
        //        DontDestroyOnLoad(this.gameObject);
        //    }
        //    else
        //    {
        //        ////Pillar info del gameManager de esta escena
        //        //GetGMInfo(levelManager, categories);
        //        //Destroy(this);
        //        instance = this;
        //        DontDestroyOnLoad(gameObject);
        //    }
        //}

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
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

            SceneManager.LoadScene("LevelMenu");
        }

        public void LoadMainMenu()
        {
            categoryIndex = 0;
            packIndex = 0;

            SceneManager.LoadScene("MainMenu");
        }

        //Si queremos borrar algo antes de q cierre
        //private void OnApplicationQuit()
        //{
            
        //}

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

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