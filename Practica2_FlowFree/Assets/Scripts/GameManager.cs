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

        public Categories selectedCategory { get; set; }
        public LevelPack selectedPack { get; set; }
        public int selectedLevel { get; set; }
        public string selectedLevelString { get; set; }

        private List<List<string>> levelStrings;

        private Color[] theme;

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

        public Color[] GetTheme()
        {
            return theme;
        }
        public void Start()
        {
#if UNITY_EDITOR
            if (categoryIndex == -1 || packIndex == -1 || levelIndex == -1 ||
                categories == null)
            {
                Debug.LogError("GameManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif

            LoadCreateProgress();

            theme = categories[categoryIndex].packs[packIndex].skin.colors;
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

        public void OnLevelFinished(int moves)
        {
            LevelProgress aux = data.categories[categoryIndex].packs[packIndex].levels[levelIndex];

            data.OnLevelCompleted(categoryIndex, packIndex, levelIndex, moves);

            Debug.Log("Guardando datos");
            saveIO.SaveData(data);  //Guardamos el progreso al acabar el nivel

            //Avisamos al LevelManager para que ponga la ventanita correspondiente
            levelManager.OnLevelFinished();
        }

        public bool NextLevel()
        {
            if(levelIndex < data.categories[categoryIndex].packs[packIndex].levels.Length - 1)
            {
                levelIndex++;
                selectedLevelString = selectedPack.levelsFile.ToString().Split('\n')[levelIndex];
                return true;
            }
            return false;
        }

        public bool PrevLevel()
        {
            if (levelIndex > 0)
            {
                levelIndex--;
                selectedLevelString = selectedPack.levelsFile.ToString().Split('\n')[levelIndex];
                return true;
            }
            return false;
        }
        public void OnHintUsed()
        {
            data.OnHintUsed();
            Debug.Log("Guardando datos");
            saveIO.SaveData(data);  //Guardamos
        }

        public void OnHintAdded()
        {
            data.OnHintAdded();
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

            //TODO: Probabilidad de anuncio intersticial ????
            interstitialAd.ShowAd();
        }

        public int GetNHints()
        {
            return data.hints;
        }

        //Para pedirlos desde el LevelManager
        public int GetSelectedLevelIndex()
        {
            return levelIndex;
        }

        public LevelPack GetSelectedPack()
        {
            return categories[categoryIndex].packs[packIndex];
        }
    }
}