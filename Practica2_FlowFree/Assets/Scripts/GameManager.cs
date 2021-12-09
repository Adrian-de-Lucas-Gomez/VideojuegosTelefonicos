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

        public int index;
        public LevelPack pack;
        public LevelManager levelManager;

        public void Start()
        {
            levelManager.initializeLevel(index, pack);
        }
    }
}