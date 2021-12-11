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
        public int categoryIndex;
        public int packIndex;
        public int levelIndex;
         
        public Categories[] categories;
        public LevelManager levelManager;

        public void Start()
        {
            levelManager.initializeLevel(levelIndex, categories[categoryIndex].packs[packIndex]);
        }
    }
}