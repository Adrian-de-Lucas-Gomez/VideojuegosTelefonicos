using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    public class CategoryCard : MonoBehaviour
    {
        //Para UI
        [SerializeField] Text categoryTitle;
        [SerializeField] Image background;
        [SerializeField] Image bottomLine;
        
        private Categories category; //El scriptable

        void Start()
        {
#if UNITY_EDITOR
            if (categoryTitle == null || background == null || bottomLine == null)
            {
                Debug.LogError("CategoryCard: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
        }

        private void Configure()
        {
            if (category == null) return;

            categoryTitle.text = category.category;
            Color aux = category.color;
            bottomLine.color = aux;
            aux.a = 110f/255f;
            background.color = aux;
        }

        public void ConfigureCategory(Categories category)
        {
            this.category = category;
            Configure();
        }
    }
}
