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

        public enum actualScene { MainMenu, SelectLevel, PlayScene };

        [SerializeField] actualScene scene;

        public ColorSkin skin;

        private string[] packStrings = null;

        //Progreso y guadado de la partida
        private ProgressData progress;
        private SaveReadWriter saveIO;

        private static GameManager instance;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                instance.SetInfo(scene);
                //AdvertisingManager.GetInstance().ReloadADS();   //Avisamos porque puede que se haya cambiado de escena

                Destroy(this);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                InitProgress();
            }
        }

        private void SetInfo(actualScene actscene)
        {
            scene = actscene;
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

        public void InitProgress()
        {
            //Creamos el lector/escritor de los datos de guardado
            saveIO = new SaveReadWriter();
            saveIO.Init();

            //Si ya habia datos guardados de antes los debemos cargar
            if (saveIO.DataFilesExist())
            {
                Debug.Log("Cargando datos de guardado desde archivo previo");

                //Tratamos de cargar el estado del archivo de guardado
                progress = saveIO.LoadData();
            }
            else    //En caso contrario se crean datos de guardado nuevo
            {
                Debug.Log("Creando datos de guardado desde cero");

                //Creamos los datos de guardado
                progress = new ProgressData();
                progress.Init(categories);
            }
        }

        public void ExitLevel()
        {
            SceneManager.LoadScene("LevelMenu");

            AdvertisingManager.GetInstance().ShowIntersticialAd();
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
            AdvertisingManager.GetInstance().HideBannerAd();
        }

        public void LoadPlayScene(int levelIndex)
        {
            this.levelIndex = levelIndex;

            SceneManager.LoadScene("PlayScene");
        }

        //Si queremos borrar algo antes de q cierre
        //private void OnApplicationQuit()
        //{
            
        //}

        public actualScene GetActualScene()
        {
            return scene;
        }

        public int NextLevel()
        {
            int levelsInPack = packStrings.Length - 1;

            if (levelIndex + 1 < levelsInPack)
            {
                return ++levelIndex;
            }

            //saveIO.SaveData(progress);

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
            progress.hints = progress.hints + 1;
            Debug.Log("Nº pistas actuales: " + progress.hints);
        }

        public void OnHintUsed()
        {
            //Se gasta una pista (si se tienen)
            if(progress.hints > 0)
            {
                progress.hints = progress.hints - 1;
                Debug.Log("Nº pistas actuales: " + progress.hints);
            }
            else Debug.Log("No tienes pistas disponibles");
        }

        public void OnLevelFinished(int numMoves)
        {
            LevelProgress levelfinished = progress.categories[categoryIndex].packs[packIndex].levels[levelIndex];
            levelfinished.completed = true;

            //Si hemos mejorado el record de movimientos previos lo actualizamos
            if(levelfinished.moveRecord > numMoves)
            {
                levelfinished.moveRecord = numMoves;
            }

            //Si hay otro nivel por delante hay que desbloquearlo
            if (levelIndex < progress.categories[categoryIndex].packs[packIndex].levels.Length - 1)
            {
                progress.categories[categoryIndex].packs[packIndex].levels[levelIndex + 1].locked = false;
            }

            //Guardamos los datos al fichero
            saveIO.SaveData(progress);
            Debug.Log("Progreso guardado");
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

        public PackProgress GetProgressInPack()
        {
            return progress.categories[categoryIndex].packs[packIndex];
        }

        public ProgressData GetProgress()
        {
            return progress;
        }

        public int GetTotalHints()
        {
            return progress.hints;
        }
    }
}