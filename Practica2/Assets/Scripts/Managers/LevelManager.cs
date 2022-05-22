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
    /// 
    public class LevelManager : MonoBehaviour
    {
        //[SerializeField] GameObject winPanel;
        [SerializeField] BoardManager boardManager;

        [SerializeField] Text levelText;
        [SerializeField] Text levelSizeText;
        //[SerializeField] RectTransform boardViewport;
        [SerializeField] Text totalFlowsText;
        [SerializeField] Text movesText;
        [SerializeField] Text bestText;
        [SerializeField] Text percentageText;
        [SerializeField] Text nHintsText;

        private int selectedLevel = 0;

        private void ConfigLevelUI(int w, int h)
        {
            GameManager gMng = GameManager.GetInstance();

            levelText.text = "level " + (selectedLevel + 1).ToString();
            levelText.color = gMng.GetSelectedCategory().color;
            levelSizeText.text = w.ToString() + "x" + h.ToString();
            //nHintsText.text = GameManager.GetInstance().GetNHints().ToString() + "x"; //TODO
        }

        public void InitializeLevel(string levelString, LevelPack pack)
        {
            (int, int) boardSize = boardManager.LoadBoard(levelString, boardScale());
            boardManager.InitializeBoard();

            ConfigLevelUI(boardSize.Item1, boardSize.Item2);
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (boardManager == null /*|| winPanel == null*/ /*|| boardViewport == null*/ || totalFlowsText == null ||
                movesText == null || bestText == null || percentageText == null || nHintsText == null ||
                levelText == null || levelSizeText == null)
            {
                Debug.LogError("LevelManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif      
            //winPanel.SetActive(false);

            //Cargamos el nivel con lo que nos diga el GameManager
            GameManager gMng = GameManager.GetInstance();
            selectedLevel = gMng.GetSelectedLevelId();
            InitializeLevel(gMng.GetSelectedLevelString(), gMng.GetSelectedPack());
        }

        public void TryNextLevel()
        {
            GameManager gMng = GameManager.GetInstance();
            selectedLevel = gMng.NextLevel(); //Pregunta a gMng si se puede pasar al siguiente nivel

            if(selectedLevel != -1) //Si existe nivel siguiente
            {
                (int, int) boardSize = boardManager.LoadBoard(gMng.GetSelectedLevelString(), boardScale());
                boardManager.StartLevelTransition();

                ConfigLevelUI(boardSize.Item1, boardSize.Item2);

                AdvertisingManager.GetInstance().ShowIntersticialAd();
            }

           
        }

        public void TryPrevLevel()
        {
            GameManager gMng = GameManager.GetInstance();
            selectedLevel = gMng.PrevLevel(); //Pregunta a gMng si se puede pasar al nivel anterior

            if (selectedLevel != -1) //Si existe nivel anterior
            {
                (int, int) boardSize = boardManager.LoadBoard(gMng.GetSelectedLevelString(), boardScale());
                boardManager.StartLevelTransition();

                ConfigLevelUI(boardSize.Item1, boardSize.Item2);

                AdvertisingManager.GetInstance().ShowIntersticialAd();
            }
        }

        public void ResetLevel()
        {
            boardManager.ResetLevel();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public void Update()
        {
            //Esto no deberia hacerse en un UPDATE

            //totalFlowsText.text = boardManager.GetNumFlows().ToString() + " /" + boardManager.GetTotalFlows().ToString();
            //movesText.text = boardManager.GetNumMoves().ToString();
            //bestText.text = GameManager.GetInstance().GetLevelRecord().ToString(); TO-DO
            //percentageText.text = ((int)boardManager.GetPercentage()).ToString() + "%";
        }

        //Calculamos el tamanho de un tile
        private float boardScale()
        {
            //TODO: esta mal
            //float aspect = boardViewport.rect.height / Screen.height;
            //float auxBoardWidth = boardViewport.rect.width / aspect;
            //float auxBoardHeight = boardViewport.rect.height / aspect;

            //El menor porque es el que se ajusta a la pantalla sin salirse
            //float auxBoardSize = Mathf.Min(auxBoardWidth, auxBoardHeight);

            //return auxBoardSize;
            return 0; //lol
        }

        public void OnLevelFinished()
        {
            //winPanel.SetActive(true);
        }

        //Al usar una pista se actualiza la UI y el numero de pistas
        public void OnUseHint()
        {
            //boardManager.UseHint();

            //GameManager.GetInstance().OnHintUsed(); TO-DO
            //nHintsText.text = GameManager.GetInstance().GetNHints().ToString() + " x";
        }
    }
}