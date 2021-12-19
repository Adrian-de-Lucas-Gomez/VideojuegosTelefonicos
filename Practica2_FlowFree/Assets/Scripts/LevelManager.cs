using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] Text totalFlowsText;
        [SerializeField] Text movesText;
        [SerializeField] Text bestText;
        [SerializeField] Text percentageText;
        [SerializeField] Text nHintsText;

        private void Start()
        {
#if UNITY_EDITOR
            if (boardManager == null || winPanel == null || totalFlowsText == null ||
                movesText == null || bestText == null || percentageText == null || nHintsText == null)
            {
                Debug.LogError("LevelManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif      
            winPanel.SetActive(false);

            nHintsText.text = GameManager.GetInstance().GetNHints().ToString() + "x";

            //Cargamos el nivel con lo que nos diga el GameManager
            GameManager auxMan = GameManager.GetInstance();

            InitializeLevel(auxMan.GetSelectedLevelIndex(), auxMan.GetSelectedPack());
        }

        public void Update()
        {
            totalFlowsText.text = boardManager.GetNumFlows().ToString() + " /" + boardManager.GetTotalFlows().ToString();
            movesText.text = boardManager.GetNumMoves().ToString();
            bestText.text = GameManager.GetInstance().GetLevelRecord().ToString();
            percentageText.text = boardManager.GetPercentage().ToString("F2") + "%";
        }

        public void InitializeLevel(int levelNumber, LevelPack pack)
        {
            string[] maps = pack.levelsFile.ToString().Split('\n');

            string level = maps[levelNumber];

            boardManager.GenerateBoard(level, pack.skin.colors);
        }

        public void OnLevelFinished()
        {
            //Aqui activamos el panel de "felicidades has ganado"
            Debug.Log("UWU resolviste el tablero");
            winPanel.SetActive(true);
        }

        //Al usar una pista se actualiza la UI y el numero de pistas
        public void OnUseHint()
        {
            boardManager.UseHint();

            GameManager.GetInstance().OnHintUsed();
            nHintsText.text = GameManager.GetInstance().GetNHints().ToString() + " x";
        }
    }
}