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
        [SerializeField] BoardManager boardManager;

        [SerializeField] Text levelText;
        [SerializeField] Text levelSizeText;
        [SerializeField] Image icon;
        [SerializeField] Sprite starSprite;
        [SerializeField] Sprite checkSprite;
        //[SerializeField] RectTransform boardViewport;
        [SerializeField] Text totalFlowsText;
        [SerializeField] Text movesText;
        [SerializeField] Text bestText;
        [SerializeField] Text percentageText;
        [SerializeField] Text nHintsText;

        [SerializeField] GameObject winPanel;
        [SerializeField] Image winPanelTopBg;
        [SerializeField] Image winPanelBorderLR;
        [SerializeField] Image winPanelBorderUD;
        [SerializeField] Text winPanelMovesText;

        [SerializeField] Button hintButton;
        //[SerializeField] Button hintButton;

        private int selectedLevel = 0;

        private void ConfigLevelUI(int w, int h)
        {
            GameManager gMng = GameManager.GetInstance();

            int levelNumber = selectedLevel % GameManager.LEVELS_PER_PAGE;

            levelText.text = "level " + (levelNumber + 1).ToString();
            levelText.color = gMng.GetSelectedCategory().color;
            levelSizeText.text = w.ToString() + "x" + h.ToString();
            nHintsText.text = GameManager.GetInstance().GetTotalHints().ToString() + "x";

            LevelProgress levelProgress = GameManager.GetInstance().GetProgressInPack().levels[GameManager.GetInstance().GetSelectedLevelId()];

            if (levelProgress.perfect)
            {
                icon.enabled = true;
                icon.sprite = starSprite;
            }
            else if (levelProgress.completed)
            {
                icon.enabled = true;
                icon.sprite = checkSprite;
            }
            else { icon.enabled = false; }

            winPanel.SetActive(false);
            winPanelTopBg.color = winPanelBorderLR.color = winPanelBorderUD.color = gMng.GetSelectedCategory().color;


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
            if (boardManager == null || winPanel == null || icon == null || starSprite == null || checkSprite == null /*|| boardViewport == null*/ || totalFlowsText == null ||
                movesText == null || bestText == null || percentageText == null || nHintsText == null ||
                levelText == null || levelSizeText == null || winPanelBorderLR == null || winPanelBorderUD == null ||
                winPanelMovesText == null || winPanelTopBg == null || hintButton == null)
            {
                Debug.LogError("LevelManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif      

            //Cargamos el nivel con lo que nos diga el GameManager
            GameManager gMng = GameManager.GetInstance();
            selectedLevel = gMng.GetSelectedLevelId();
            InitializeLevel(gMng.GetSelectedLevelString(), gMng.GetSelectedPack());
            hintButton.onClick.AddListener(PlayAddForHint);
        }

        //public void Init(int levelID, LevelPack pack)
        //{
        //    //Cargamos el nivel con lo que nos diga el GameManager
        //    GameManager gMng = GameManager.GetInstance();
        //    selectedLevel = levelID;
        //    InitializeLevel(gMng.GetSelectedLevelString(), pack);
        //}

        public void TryNextLevel()
        {
            GameManager gMng = GameManager.GetInstance();

            if (!gMng.NextLevelAvailable()) return; //Si el siguente no está desbloqueado no se pasa

            LevelProgress levelProgress = gMng.GetProgressInPack().levels[gMng.GetSelectedLevelId()];

            selectedLevel = gMng.NextLevel(); //Pregunta a gMng si se puede pasar al siguiente nivel

            if(selectedLevel != -1) //Si existe nivel siguiente
            {
                (int, int) boardSize = boardManager.LoadBoard(gMng.GetSelectedLevelString(), boardScale());
                boardManager.StartLevelTransition();

                ConfigLevelUI(boardSize.Item1, boardSize.Item2);
                UpdateUIelements();

                AdvertisingManager.GetInstance().ShowIntersticialAd();
            }
            else
            {
                //Si no hay siguiente siguiente nivel en el pack te devuelve al menu de seleccion de nivel
                GameManager.GetInstance().ExitLevel();
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
                UpdateUIelements();

                AdvertisingManager.GetInstance().ShowIntersticialAd();
            }
        }

        public void ResetLevel()
        {
            UpdateUIelements();
            boardManager.ResetLevel();
            winPanel.SetActive(false);
        }

        public void SetActiveWinPanel()
        {
            winPanel.SetActive(true);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public void Update()
        {
            //Esto no deberia hacerse en un UPDATE

            //UpdateUIelements(); //Se que no va aquí, es temporal no me peguen por favor
        }

        public void UpdateHintCount(int hints)
        {
            nHintsText.text = hints.ToString() + "x";
        }

        public void UpdateUIelements()
        {
            totalFlowsText.text = boardManager.GetNumFlows().ToString() + " /" + boardManager.GetTotalFlows().ToString();
            movesText.text = boardManager.GetNumMoves().ToString();
            string recordText = "-";
            if (GameManager.GetInstance().GetProgressInPack().levels[GameManager.GetInstance().GetSelectedLevelId()].moveRecord > 0)
            {
                bestText.text = GameManager.GetInstance().GetProgressInPack().levels[GameManager.GetInstance().GetSelectedLevelId()].moveRecord.ToString();
            }
            else bestText.text = recordText;
            percentageText.text = ((int)boardManager.GetPercentage()).ToString() + "%";
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

        //Al usar una pista se actualiza la UI y el numero de pistas
        public void OnUseHint()
        {
            //Miramos si podemos gastar pistas
            if (GameManager.GetInstance().OnHintUsed())
            {
                boardManager.UseHint();
                nHintsText.text = GameManager.GetInstance().GetTotalHints().ToString() + " x";
            }
            //No se hace nada si no hay pistas
        }

        public void PlayAddForHint()
        {
            //Hacemos play del anuncio para obtener una pista
            AdvertisingManager.GetInstance().ShowRewardedAd();
            //Actualizamos el número de pistas disponibles
            nHintsText.text = GameManager.GetInstance().GetTotalHints().ToString() + " x";
        }

        public void OnLevelFinished(int numMoves, int numFlows)
        {
            winPanelMovesText.text = "You completed the level in " + numMoves.ToString() + " moves.";
            GameManager.GetInstance().OnLevelFinished(numMoves, numFlows);
        }
    }
}