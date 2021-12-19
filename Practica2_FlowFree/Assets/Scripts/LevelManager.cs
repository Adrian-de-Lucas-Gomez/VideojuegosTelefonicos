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

        [SerializeField] Text levelText;
        [SerializeField] Text levelSizeText;
        [SerializeField] RectTransform boardViewport;
        [SerializeField] Text totalFlowsText;
        [SerializeField] Text movesText;
        [SerializeField] Text bestText;
        [SerializeField] Text percentageText;
        [SerializeField] Text nHintsText;

        private void Start()
        {
#if UNITY_EDITOR
            if (boardManager == null || winPanel == null || boardViewport == null || totalFlowsText == null ||
                movesText == null || bestText == null || percentageText == null || nHintsText == null ||
                levelText == null || levelSizeText == null)
            {
                Debug.LogError("LevelManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif      
            winPanel.SetActive(false);

            //Cargamos el nivel con lo que nos diga el GameManager
            GameManager auxMan = GameManager.GetInstance();
            InitializeLevel(auxMan.selectedLevelString, auxMan.GetSelectedPack());
        }

        public void Update()
        {
            totalFlowsText.text = boardManager.GetNumFlows().ToString() + " /" + boardManager.GetTotalFlows().ToString();
            movesText.text = boardManager.GetNumMoves().ToString();
            bestText.text = GameManager.GetInstance().GetLevelRecord().ToString();
            percentageText.text = ((int)boardManager.GetPercentage()).ToString() + "%";
        }

        //Calculamos el tamanho de un tile
        private float boardScale()
        {
            //TODO: esta mal
            float aspect = boardViewport.rect.height / Screen.height;
            float auxBoardWidth = boardViewport.rect.width / aspect;
            float auxBoardHeight = boardViewport.rect.height / aspect;

            //El menor porque es el que se ajusta a la pantalla sin salirse
            float auxBoardSize = Mathf.Min(auxBoardWidth, auxBoardHeight);

            return auxBoardSize;
        }

        public void InitializeLevel(string levelString, LevelPack pack)
        {
            string[] maps = pack.levelsFile.ToString().Split('\n');

            (int, int) boardSize = boardManager.GenerateBoard(levelString, pack.skin.colors, boardScale());

            ConfigLevelUI(boardSize.Item1, boardSize.Item2);
        }

        private void ConfigLevelUI(int w, int h)
        {
            GameManager auxMan = GameManager.GetInstance();

            levelText.text = "level " + auxMan.selectedLevel.ToString();
            levelText.color = auxMan.selectedCategory.color;
            levelSizeText.text = w.ToString() + "x" + h.ToString();
            nHintsText.text = GameManager.GetInstance().GetNHints().ToString() + "x";
        }

        public void TryNextLevel()
        {
            GameManager auxMan = GameManager.GetInstance();
            auxMan.NextLevel();
            InitializeLevel(auxMan.selectedLevelString, auxMan.GetSelectedPack());
        }

        public void TryPrevLevel()
        {
            GameManager auxMan = GameManager.GetInstance();
            auxMan.PrevLevel();
            InitializeLevel(auxMan.selectedLevelString, auxMan.GetSelectedPack());
        }

        public void ResetLevel()
        {
            boardManager.ResetLevel();
        }

        public void OnLevelFinished()
        {
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