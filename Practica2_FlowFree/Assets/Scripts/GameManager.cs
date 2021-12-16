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
    }
}