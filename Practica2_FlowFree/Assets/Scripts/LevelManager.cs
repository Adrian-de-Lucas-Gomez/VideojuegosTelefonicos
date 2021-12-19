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
        [SerializeField] GameObject winPanel;
        [SerializeField] BoardManager boardManager;

        private void Start()
        {
#if UNITY_EDITOR
            if (boardManager == null)
            {
                Debug.LogError("LevelManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
            if(winPanel == null)
            {
                Debug.LogError("LevelManager: No se asigno el panel de victoria desde el editor.");
                return;
            }
#endif      
            winPanel.SetActive(false);
        }

        public void initializeLevel(int levelNumber, LevelPack pack)
        {
            string[] maps = pack.levelsFile.ToString().Split('\n');

            string level = maps[levelNumber];

            boardManager.GenerateBoard(level, pack.skin.colors);
        }

        public void onLevelFinished()
        {
            //Aqui activamos el panel de "felicidades has ganado"
            Debug.Log("UWU resolviste el tablero");
            winPanel.SetActive(true);
        }
    }
}