using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    public class LevelButton : MonoBehaviour
    {
        //Para UI
        [SerializeField] Text levelNumberText;
        [SerializeField] Image background;
        [SerializeField] Image leftRightLines;
        [SerializeField] Image upDownLines;

        private int levelIndex;
        private string levelString;
        private bool _locked;

        void Start()
        {
#if UNITY_EDITOR
            if (levelNumberText == null || background == null || leftRightLines == null || upDownLines == null)
            {
                Debug.LogError("LevelButton: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
        }

        public void ConfigureLevelButton(int id, Color color, string levelString, bool locked)
        {
            levelIndex = id;
            this.levelString = levelString;

            levelNumberText.text = levelIndex.ToString();

            _locked = locked;

            if (!locked)
            {
                Color aux = color;
                leftRightLines.color = aux;
                upDownLines.color = aux;
                aux.a = 110f / 255f;
                background.color = aux;

            }
            else
            {
                Color aux = Color.grey;
                leftRightLines.color = aux;
                upDownLines.color = aux;
                aux.a = 110f / 255f;
                background.color = aux;
            }
        }

        public void OnChooseLevel()
        {
            if (!_locked) {
                GameManager gMng = GameManager.GetInstance();
                gMng.selectedLevel = levelIndex;
                gMng.selectedLevelString = levelString;
                gMng.ChangeScene("GameScene");
            }
            else
            {
                Debug.Log("Nivel no desbloqueado");
            }
        }
    }
}
