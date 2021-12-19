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
        [SerializeField] int categoryIndex = -1;
        [SerializeField] int packIndex = -1;
        [SerializeField] int levelIndex = -1;

        [SerializeField] Categories[] categories;
        [SerializeField] LevelManager levelManager;

        [SerializeField] ProgressData data;
        [SerializeField] SaveReadWriter saveIO;

        [SerializeField] InterstitialAd interstitialAd;

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
                //Pillar info del gameManager de esta escena
                GetGMInfo(levelManager, categories);
                Destroy(this);
            }
        }

        private void GetGMInfo(LevelManager levelManager, Categories[] categories)
        {
            instance.categories = categories;
            instance.levelManager = levelManager;
        }

        public static GameManager GetInstance()
        {
            return instance;
        }

        public void Start()
        {
#if UNITY_EDITOR
            if (categoryIndex == -1 || packIndex == -1 || levelIndex == -1 ||
                categories == null || levelManager == null)
            {
                Debug.LogError("GameManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif

            LoadCreateProgress();

            //Esto se debe de llamar cuando se cambie a una GameScene

            levelManager.initializeLevel(levelIndex, categories[categoryIndex].packs[packIndex]);
        }

        public void LoadCreateProgress()
        {
            saveIO = new SaveReadWriter();
            saveIO.Init();

            //Para limpiar archivo de guardado

            //data = new ProgressData();
            //data.Init(categories);
            //saveIO.SaveData(data);
            Debug.Log("Procediendo");
            data = null;    //Para evitar problemas

            if (File.Exists("Assets/SaveFile.json"))
            {
                data = saveIO.LoadData();
                Debug.Log("Cargados los datos");
            }

            if (data == null) { 
                Debug.Log("Creando nuevos datos");
                data = new ProgressData();
                data.Init(categories);
            }
        }

        public void onLevelFinished(int moves)
        {
            LevelProgress aux = data.categories[categoryIndex].packs[packIndex].levels[levelIndex];

            data.onLevelCompleted(categoryIndex, packIndex, levelIndex, moves);

            Debug.Log("Guardando datos");
            saveIO.SaveData(data);  //Guardamos el progreso al acabar el nivel

            //Avisamos al LevelManager para que ponga la ventanita correspondiente
            levelManager.onLevelFinished();
        }

        public void onHintUsed()
        {
            data.onHintUsed();
            Debug.Log("Guardando datos");
            saveIO.SaveData(data);  //Guardamos
        }

        public void onHintAdded()
        {
            data.onHintAdded();
            Debug.Log("Guardando datos");
            saveIO.SaveData(data);  //Guardamos
        }

        public int GetLevelRecord()
        {
            return data.categories[categoryIndex].packs[packIndex].levels[levelIndex].moveRecord;
        }

        public void ChangeScene(string name)
        {
            SceneManager.LoadScene(name, LoadSceneMode.Single);

            //Probabilidad de anuncio intersticial ????
            interstitialAd.ShowAd();
        }
    }
}