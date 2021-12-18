using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Destroy(this);
            }
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
            levelManager.initializeLevel(levelIndex, categories[categoryIndex].packs[packIndex]);
            data = new ProgressData();
            data.Init(categories);

            saveIO = new SaveReadWriter();
            saveIO.Init();
            saveIO.SaveData(data);

            data = saveIO.LoadData();
            if (data == null) { Debug.Log("No cargó bien los archivos"); }
        }

        public void onLevelFinished(int moves)
        {
            LevelProgress aux = data.categories[categoryIndex].packs[packIndex].levels[levelIndex];

            if (aux.completed)  //Si lo estamos rejugando
            {
                if (moves < aux.moveRecord)  //Si mejoramos el record previo
                {
                    aux.moveRecord = moves;
                }
            }
            else    //Si es la primera vez que lo jugamos
            {
                aux.completed = true;
                aux.moveRecord = moves;
            }
            Debug.Log("Guardadndo datos");
            saveIO.SaveData(data);  //Guardamos el progreso al acabar el nivel
        }
    }
}