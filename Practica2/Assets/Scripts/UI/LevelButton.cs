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
        [SerializeField] Image icon;
        [SerializeField] Sprite lockSprite;
        [SerializeField] Sprite tickSprite;
        [SerializeField] Sprite starSprite;

        [SerializeField] Button button;

        private int levelIndex;
        private string levelString;
        private bool _locked;

        void Start()
        {
#if UNITY_EDITOR
            if (levelNumberText == null || background == null || leftRightLines == null || upDownLines == null
                || icon == null || lockSprite == null || tickSprite == null || starSprite == null || button == null)
            {
                Debug.LogError("LevelButton: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
        }

        public void ConfigureLevelButton(int id, int interfaceID, Color color, string levelString, bool locked, bool completed)
        {
            levelIndex = id;
            this.levelString = levelString;

            levelNumberText.text = interfaceID.ToString();

            _locked = locked;

            if (!locked)
            {
                button.enabled = true;

                Color aux = color;
                leftRightLines.color = aux;
                upDownLines.color = aux;
                icon.color = aux;
                aux.a = 110f / 255f;
                background.color = aux;
                icon.enabled = false;
                levelNumberText.enabled = true;
            }
            else
            {
                button.enabled = false;

                Color aux = Color.grey;
                leftRightLines.color = aux;
                upDownLines.color = aux;
                icon.color = aux;
                aux.a = 110f / 255f;
                background.color = aux;
                icon.enabled = true;
                icon.sprite = lockSprite;
                levelNumberText.enabled = false;
            }


            if (completed) {
                icon.enabled = true;
                icon.sprite = tickSprite;
            }
        }

        public void OnClick()
        {
            //En el nivel van de 1-150 pero logicamente son 0-149
            GameManager.GetInstance().LoadPlayScene(levelIndex - 1);
        }
    }
}
