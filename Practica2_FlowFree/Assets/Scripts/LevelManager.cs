using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    /// <summary>
    /// Se encarga de la escena del nivel
    /// Cuando BoardManager termina el puzzle muestra la UI correspondiente al cambio de nivel
    /// Gestiona los botones de anuncios y pistas
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public BoardManager boardManager;
        private void Start()
        {
            
        }

        public void initializeLevel(int levelNumber, LevelPack pack)
        {
            string[] maps = pack.levelsString.ToString().Split('\n');

            string level = maps[levelNumber];

            boardManager.GenerateBoard(level, pack.colors);
        }
    }
}