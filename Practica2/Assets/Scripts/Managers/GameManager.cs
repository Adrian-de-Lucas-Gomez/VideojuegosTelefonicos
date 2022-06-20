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
    
    public struct CurrentLevel{
        public int lvlIndex;
        public int pckIndex;
        public int catIndex;
        public int sce;
    }
    
    public class GameManager : MonoBehaviour
    {
        [SerializeField] int categoryIndex = 0;
        [SerializeField] int packIndex = 0;
        [SerializeField] int levelIndex = 0;

        [SerializeField] int skinIndex = 0;

        [SerializeField] List<Categories> categories;

        [SerializeField] LevelManager levelManager;

        public const int LEVELS_PER_PAGE = 30;

        public enum actualScene { MainMenu=0, SelectLevel=1, PlayScene=2 };

        [SerializeField] actualScene scene;

        [SerializeField] ColorSkin[] skin;

        private string[] packStrings = null;

        private float[] posInScroll;

        //Progreso y guadado de la partida
        private ProgressData progress;
        private SaveReadWriter saveIO;

        private static GameManager instance;


        private CurrentLevel aux;
        void Awake()
        {

            if (instance != null && instance != this)
            {
                //Avisamos a la instancia y le damos los valores y referencias del GameManager de la escena
                instance.SetInfo(scene, categories, levelManager, skin);
                instance.InitLevelManager();

                instance.SaveLevelPlaying();

                Destroy(this.gameObject);
            }
            else
            {
                //Es la primera instancia del GameManager
                instance = this;
                DontDestroyOnLoad(gameObject);

                InitProgress();

                //PlayerPrefs.DeleteAll();

                aux = LoadLevelPlaying();

                if (aux.lvlIndex == -1 || aux.pckIndex == -1 || aux.catIndex == -1)
                {
                    //Nada
                    Debug.Log("hola");
                }
                else
                {
                    levelIndex = aux.lvlIndex;
                    packIndex = aux.pckIndex;
                    categoryIndex = aux.catIndex;
                }

                InitLevelManager();

                //ScrollSaves();

            }
        }

        private void SetInfo(actualScene actscene, List<Categories> cat, LevelManager lvlManager, ColorSkin[] skins)
        {
            scene = actscene;
            categories = cat;
            levelManager = lvlManager;
            skin = skins;
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
            if (aux.sce == 2)
            {
                LoadPlayScene(aux.lvlIndex);
            }

            //packStrings = GetSelectedPack().levelsFile.ToString().Split('\n');
        }

        //private void ScrollSaves()
        //{
        //    int totalLevels = 0;
        //    for (int i = 0; i < categories.Count; i++)
        //    {
        //        for (int j = 0; j < categories[i].packs.Count; j++)
        //        {
        //            totalLevels++;
        //        }
        //    }

        //    posInScroll = new float[totalLevels];

        //    for (int i = 0; i < totalLevels; i++)
        //    {
        //        posInScroll[i] = 0.0f;
        //    }
        //}

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
                //Debug.Log("Cargando datos de guardado desde archivo previo");

                //Tratamos de cargar el estado del archivo de guardado
                progress = saveIO.LoadData();

                if (progress == null)   //El archivo se ha tocado externamente o se ha corrompido
                {
                    //Creamos los datos de guardado de nuevo
                    //Debug.Log("Creando datos de guardado nuevos por pirata ;) ");
                    progress = new ProgressData();
                    progress.Init(categories);
                }
            }
            else    //En caso contrario se crean datos de guardado nuevo
            {
                //Debug.Log("Creando datos de guardado desde cero");

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
                levelManager.LoadLevel(levelIndex, GetSelectedPack());

                SaveLevelPlaying();
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

            saveIO.SaveData(progress);

            SceneManager.LoadScene("MainMenu");
        }

        public void LoadPlayScene(int lvlIndex)
        {
            levelIndex = lvlIndex;

            saveIO.SaveData(progress);

            SceneManager.LoadScene("PlayScene");

            SaveLevelPlaying();
        }

        //public void SetLevelIndex(int lvl)
        //{
        //    levelIndex = lvl;
        //}

        //public void InitLevel(int lvlIndex, LevelPack pack)
        //{
        //    levelManager.LoadLevel(lvlIndex, pack);
        //}

        public void DeactivateADS()
        {
            AdvertisingManager.DeactivateADS();
        }

        //Si queremos borrar algo antes de q cierre
        //private void OnApplicationQuit()
        //{

        //}

        public void SetPosInLevelSelection(float posScroll)
        {
            PlayerPrefs.SetFloat("ScrollPos" + (categoryIndex + packIndex), posScroll);
            //posInScroll[categoryIndex + packIndex] = posScroll;
        }

        public float GetPosInLevelSelection()
        {
            //return posInScroll[categoryIndex + packIndex];
            return PlayerPrefs.GetFloat("ScrollPos" + (categoryIndex + packIndex), 0.0f);
        }

        public actualScene GetActualScene()
        {
            return scene;
        }

        public int NextLevel()
        {
            int levelsInPack = packStrings.Length - 1;

            if (levelIndex + 1 < levelsInPack)
            {
                levelIndex += 1;
                return levelIndex;
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

        public void NextLevelNotPerfectMenu(Categories packCategory, LevelPack packs)
        {
            categoryIndex = categories.IndexOf(packCategory);
            packIndex = categories[categoryIndex].packs.IndexOf(packs);

            int index = GetNextLevelNotPerfect();

            if (index != -1) LoadPlayScene(index);
        }

        public int GetNextLevelNotPerfect()
        {
            int index = 0;

            while (index < progress.categories[categoryIndex].packs[packIndex].levels.Length &&
                !progress.categories[categoryIndex].packs[packIndex].levels[index].locked &&
                progress.categories[categoryIndex].packs[packIndex].levels[index].perfect)
            {
                index++;
            }
            //Si se ha pasado de largo
            if (index == progress.categories[categoryIndex].packs[packIndex].levels.Length) index = -1;
            else levelIndex = index;

            return index;
        }

        public bool IsThereNextLevelNotPerfect()
        {
            int index = 0;

            while (index < progress.categories[categoryIndex].packs[packIndex].levels.Length &&
                !progress.categories[categoryIndex].packs[packIndex].levels[index].locked &&
                progress.categories[categoryIndex].packs[packIndex].levels[index].perfect)
            {
                index++;
            }
            //Si se ha pasado de largo
            if (index == progress.categories[categoryIndex].packs[packIndex].levels.Length) return false;
            else return true;
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
                levelIndex -= 1;
                return levelIndex;
            }

            return -1; //-1 es que no existe nivel anterior
        }

        public void OnHintAdded()
        {
            //Añadir una pista
            progress.OnHintAdded();
            //Debug.Log("Nº pistas actuales: " + progress.hints);

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
                //Debug.Log("Nº pistas actuales: " + progress.hints);
                //Guardamos que se ha usado la pista
                saveIO.SaveData(progress);

                return true;
            }
            else
            {
                //
                //("No tienes pistas disponibles");
                return false;
            }

            
        }

        public void OnLevelFinished(int numMoves, int numFlows)
        {
            progress.OnLevelCompleted(categoryIndex, packIndex, levelIndex, numMoves, numFlows);

            //Guardamos los datos al fichero
            saveIO.SaveData(progress);
            //Debug.Log("Progreso guardado");
        }

        public void SaveLevelPlaying()
        {
            PlayerPrefs.SetInt("LevelIndex", levelIndex);
            PlayerPrefs.SetInt("PaxkIndex", packIndex);
            PlayerPrefs.SetInt("CategoryIndex", categoryIndex);
            PlayerPrefs.SetInt("Scene", (int)scene);

            //Debug.Log("Guardando pantalla actual");
        }

        public CurrentLevel LoadLevelPlaying()
        {
            CurrentLevel indexes = new CurrentLevel();

            indexes.lvlIndex = PlayerPrefs.GetInt("LevelIndex", -1);
            indexes.pckIndex = PlayerPrefs.GetInt("PaxkIndex", -1);
            indexes.catIndex = PlayerPrefs.GetInt("CategoryIndex", -1);
            indexes.sce = PlayerPrefs.GetInt("Scene", 0);

            return indexes;
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public static GameManager GetInstance()
        {
            return instance;
        }

        public Color GetWallColorFromPack()
        {
            return Color.cyan;
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

        public void SetTheme(int colorIndex)
        {
            skinIndex = colorIndex;
            SceneManager.LoadScene("MainMenu");
        }

        public Color[] GetTheme()
        {
            return skin[skinIndex].colors;
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