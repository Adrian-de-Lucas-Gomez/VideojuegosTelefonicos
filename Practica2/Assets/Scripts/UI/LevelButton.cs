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

        private int levelIndex;
        private string levelString;
        private bool _locked;

        void Start()
        {
#if UNITY_EDITOR
            if (levelNumberText == null || background == null || leftRightLines == null || upDownLines == null
                || icon == null || lockSprite == null || tickSprite == null || starSprite == null)
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
                icon.color = aux;
                aux.a = 110f / 255f;
                background.color = aux;
                icon.enabled = false;
                levelNumberText.enabled = true;
            }
            else
            {
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
        }

        public void OnClick()
        {
            //En el nivel van de 1-150 pero logicamente son 0-149
            GameManager.GetInstance().LoadPlayScene(levelIndex - 1);
        }
    }
}
