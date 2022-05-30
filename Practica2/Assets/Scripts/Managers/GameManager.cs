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

        [SerializeField] LevelManager levelManager;

        public const int LEVELS_PER_PAGE = 30;

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
                //Avisamos a la instancia y le damos los valores y referencias del GameManager de la escena
                instance.SetInfo(scene, categories, levelManager);
                instance.InitLevelManager();

                Destroy(gameObject);
            }
            else
            {
                //Es la primera instancia del GameManager
                instance = this;
                DontDestroyOnLoad(gameObject);

                InitProgress();

                InitLevelManager();
            }
        }

        private void SetInfo(actualScene actscene, List<Categories> cat, LevelManager lvlManager)
        {
            scene = actscene;
            categories = cat;
            levelManager = lvlManager;
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
            //packStrings = GetSelectedPack().levelsFile.ToString().Split('\n');
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

                if (progress == null)   //El archivo se ha tocado externamente o se ha corrompido
                {
                    //Creamos los datos de guardado de nuevo
                    Debug.Log("Creando datos de guardado nuevos por pirata ;) ");
                    progress = new ProgressData();
                    progress.Init(categories);
                }
            }
            else    //En caso contrario se crean datos de guardado nuevo
            {
                Debug.Log("Creando datos de guardado desde cero");

                //Creamos los datos de guardado
                progress = new ProgressData();
                progress.Init(categories);
            }



            //Guardamos el progreso para generar el archivo (o sobreescribirlo si estaba)
            saveIO.SaveData(progress);
        }

        public void InitLevelManager()
        {
            if (scene == actualScene.PlayScene && levelManager != null)
            {
                packStrings = GetSelectedPack().levelsFile.ToString().Split('\n');
                levelManager.Init(levelIndex, GetSelectedPack());
            }
            //Si no es la escena o no hay level manager se ignora la orden
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
        }

        public void LoadMainMenu()
        {
            //Al volver al menu principal se les da un valor por defecto
            categoryIndex = 0;
            packIndex = 0;
            levelIndex = 0;
            packStrings = categories[categoryIndex].packs[0].levelsFile.ToString().Split('\n');

            SceneManager.LoadScene("MainMenu");
        }

        public void LoadPlayScene(int lvlIndex)
        {
            levelIndex = lvlIndex;
            SceneManager.LoadScene("PlayScene");
        }

        public void InitLevel(int lvlIndex, LevelPack pack)
        {
            levelManager.Init(lvlIndex, pack);
        }

        public void DeactivateADS()
        {
            AdvertisingManager.DeactivateADS();
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

            return -1; //-1 es que no existe nivel siguiente
        }

        public bool NextLevelAvailable()
        {
            //Si hay otro nivel por delante hay que desbloquearlo
            if (levelIndex + 1 < progress.categories[categoryIndex].packs[packIndex].levels.Length - 1 &&
                !progress.categories[categoryIndex].packs[packIndex].levels[levelIndex + 1].locked || 
                levelIndex + 1 == progress.categories[categoryIndex].packs[packIndex].levels.Length - 1)    //Caso especial si es el último nivel del pack
            {
                return true;
            }
            else return false;
        }

        public bool PrevLevelAvailable()
        {
            if (levelIndex - 1 >= 0)
            {
                return true;
            }
            else return false;
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
            progress.OnHintAdded();
            Debug.Log("Nº pistas actuales: " + progress.hints);

            //Avisamos al levelManager de la nueva pista
            levelManager.UpdateHintCount(progress.hints);

            //Guardamos que se ha usado la pista
            saveIO.SaveData(progress);
        }

        public bool OnHintUsed()    //True si se ha canjeado la pista, false si no
        {
            //Se gasta una pista (si se tienen)
            if (progress.hints > 0)
            {
                progress.OnHintUsed();
                Debug.Log("Nº pistas actuales: " + progress.hints);
                //Guardamos que se ha usado la pista
                saveIO.SaveData(progress);

                return true;
            }
            else
            {
                Debug.Log("No tienes pistas disponibles");
                return false;
            }

            
        }

        public void OnLevelFinished(int numMoves, int numFlows)
        {
            progress.OnLevelCompleted(categoryIndex,packIndex,levelIndex, numMoves, numFlows);

            //LevelProgress levelfinished = progress.categories[categoryIndex].packs[packIndex].levels[levelIndex];
            //levelfinished.completed = true;

            ////Si no habia record previo o hemos mejorado el record de movimientos previo lo actualizamos
            //if(levelfinished.moveRecord <= 0 || levelfinished.moveRecord > numMoves)
            //{
            //    levelfinished.moveRecord = numMoves;
            //}

            ////Miramos si se ha hecho perfecto el nivel
            //if(numMoves == numFlows) levelfinished.perfect = true;
            //else levelfinished.perfect = false;

            ////Si hay otro nivel por delante hay que desbloquearlo
            //if (levelIndex < progress.categories[categoryIndex].packs[packIndex].levels.Length - 1)
            //{
            //    progress.categories[categoryIndex].packs[packIndex].levels[levelIndex + 1].locked = false;
            //}

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