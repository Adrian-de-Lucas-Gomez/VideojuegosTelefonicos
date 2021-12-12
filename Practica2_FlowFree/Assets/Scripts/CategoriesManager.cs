using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    /// <summary>
    /// Crea las categorias de niveles en el canvas
    /// </summary>

    public class CategoriesManager : MonoBehaviour
    {
        [SerializeField] Categories[] categories;
        [SerializeField] Transform categoriesParent;
        [SerializeField] CategoryCard categoryCardPrefab;

        void Start()
        {
#if UNITY_EDITOR
            if (categories.Length == 0 || categoriesParent == null || categoryCardPrefab == null)
            {
                Debug.LogError("CategoriesManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
            //Crear las categorias de niveles
            for (int i = 0; i < categories.Length; i++)
            {
                int index = i;

                CategoryCard card = Instantiate(categoryCardPrefab, categoriesParent);
                card.ConfigureCategory(categories[i]);
                //card.button.onClick.AddListener(() =>
                //{
                //    SelectCategory(index);
                //});

                ////If it's not unlocked it can't be selected
                //if (!ProgressManager.Instance.IsCategoryUnlocked(index))
                //{
                //    card.button.enabled = false;
                //    card.image.sprite = deactivatedImage;
                //    card.button.image.color = deactivatedCategoryColor;
                //}
            }
        }
    }
}