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
        [SerializeField] Button nextNotPerfectButton;
        [SerializeField] Button cheatButton;
        private int pushButtonCounter = 0;
        private float cheatCounter = 0.0f;
        private bool cheatingTime = false;

        int levelIDtemp;
        LevelPack packtemp;

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

            icon.color = gMng.GetSelectedCategory().color;

            if (levelProgress.perfect)  //Nivel completado con el numero minimo de pasos
            {
                icon.enabled = true;
                icon.sprite = starSprite;
            }
            else if (levelProgress.completed)   //Nivel completado de forma no optima
            {
                icon.enabled = true;
                icon.sprite = checkSprite;
            }
            else { icon.enabled = false; }  //Nivel no completado todavia

            winPanel.SetActive(false);
            winPanelTopBg.color = winPanelBorderLR.color = winPanelBorderUD.color = gMng.GetSelectedCategory().color;
        }

        public void InitializeLevel(string levelString, LevelPack pack)
        {
            (int, int) boardSize = boardManager.LoadBoard(levelString);

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
            selectedLevel = levelIDtemp;
            InitializeLevel(gMng.GetSelectedLevelString(), packtemp);
            //Arrancamos el board
            boardManager.InitializeBoard(gMng.GetWallColorFromPack());
            hintButton.onClick.AddListener(PlayAddForHint);
            cheatButton.onClick.AddListener(Cheat);

            UpdateUIelements();

            gMng.SaveLevelPlaying();
        }

        public void LoadLevel(int levelID, LevelPack pack)
        {
            //Identificador
            levelIDtemp = levelID;
            //Pack al que pertenece
            packtemp = pack;
        }


        public void TryNextLevel()
        {
            GameManager gMng = GameManager.GetInstance();

            //if (!gMng.NextLevelAvailable()) return; //Si el siguente no está desbloqueado no se pasa

            //gMng.SetLevelIndex(selectedLevel);
            selectedLevel = gMng.NextLevel(); //Pregunta a gMng si se puede pasar al siguiente nivel

            if(selectedLevel != -1) //Si existe nivel siguiente
            {
                boardManager.StartLevelTransition();
                InitializeLevel(gMng.GetSelectedLevelString(), gMng.GetSelectedPack());
                UpdateUIelements();

                gMng.SaveLevelPlaying();

                AdvertisingManager.GetInstance().ShowIntersticialAd();
            }
            else
            {
                //Si no hay siguiente siguiente nivel en el pack te devuelve al menu de seleccion de nivel
                GameManager.GetInstance().ExitLevel();
            }
        }

        public void TryNextNotPerfectLevel()
        {
            GameManager gMng = GameManager.GetInstance();

            selectedLevel = gMng.GetNextLevelNotPerfect();

            if (selectedLevel != -1) //Si existe nivel siguiente
            {
                boardManager.StartLevelTransition();
                InitializeLevel(gMng.GetSelectedLevelString(), gMng.GetSelectedPack());
                UpdateUIelements();

                gMng.SaveLevelPlaying();
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

            //gMng.SetLevelIndex(selectedLevel);
            selectedLevel = gMng.PrevLevel(); //Pregunta a gMng si se puede pasar al nivel anterior

            if (selectedLevel != -1) //Si existe nivel anterior
            {
                boardManager.StartLevelTransition();
                InitializeLevel(gMng.GetSelectedLevelString(), gMng.GetSelectedPack());
                UpdateUIelements();

                gMng.SaveLevelPlaying();

                AdvertisingManager.GetInstance().ShowIntersticialAd();
            }
        }

        public void ResetLevel()
        {
            boardManager.ResetLevel();
            winPanel.SetActive(false);
            UpdateUIelements();
        }

        public void SetActiveWinPanel()
        {
            winPanel.SetActive(true);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public void Update()
        {
            if (cheatingTime) cheatCounter += Time.deltaTime;

            if (cheatCounter > 1.0f)
            {
                cheatCounter = 0.0f;
                pushButtonCounter = 0;
                cheatingTime = false;
            }
        }

        public void UpdateHintCount(int hints)
        {
            nHintsText.text = hints.ToString() + "x";
        }

        public void UpdateUIelements()
        {
            GameManager gMng = GameManager.GetInstance();

            totalFlowsText.text = boardManager.GetNumFlows().ToString() + " /" + boardManager.GetTotalFlows().ToString();
            movesText.text = boardManager.GetNumMoves().ToString();
            string recordText = "-";
            if (gMng.GetProgressInPack().levels[GameManager.GetInstance().GetSelectedLevelId()].moveRecord > 0)
            {
                bestText.text = gMng.GetProgressInPack().levels[GameManager.GetInstance().GetSelectedLevelId()].moveRecord.ToString();
            }
            else bestText.text = recordText;
            percentageText.text = ((int)boardManager.GetPercentage()).ToString() + "%";

            if(!gMng.IsThereNextLevelNotPerfect() || selectedLevel == gMng.GetSelectedLevelId()) //Si no hay niveles para saltar
            {
                nextNotPerfectButton.interactable = false;
            }
        }

        public void BackButton()
        {
            GameManager.GetInstance().ExitLevel();
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

        public void Cheat()
        {
            if (!cheatingTime)
            {
                cheatingTime = true;
            }

            pushButtonCounter++;

            if (pushButtonCounter >= 3)
            {
                for(int i=0; i<boardManager.GetTotalFlows(); i++) boardManager.UseHint();
                OnLevelFinished(boardManager.GetTotalFlows(), boardManager.GetTotalFlows());
            }
            else
            {
                Debug.Log("En " + (3 - pushButtonCounter) + " pulsaciones acabas el trucazo");
            }
            
            
        }
    }
}