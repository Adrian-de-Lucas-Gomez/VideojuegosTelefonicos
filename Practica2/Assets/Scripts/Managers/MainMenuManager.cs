using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    /// <summary>
    /// Gestiona los menus del juego
    /// </summary>

    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] Transform categoriesParent;
        [SerializeField] CategoryCard categoryCardPrefab;
        [SerializeField] LevelPackCard levelPackCardPrefab;
        [SerializeField] GameObject titleParent;

        [SerializeField] Button adsDesactivationButton;

        public void LoadMainMenu()
        {
            List<Categories> categories = GameManager.GetInstance().GetCategories();
            //Crear las categorias de niveles
            for (int i = 0; i < categories.Count; i++)
            {
                CategoryCard card = Instantiate(categoryCardPrefab, categoriesParent);
                card.ConfigureCategory(categories[i]);


            //Crear los packs de niveles dentro de cada categoria
            List<LevelPack> packs = categories[i].packs;
                for (int j = 0; j < packs.Count; ++j)
                {
                    LevelPackCard pack = Instantiate(levelPackCardPrefab, categoriesParent);

                    PackProgress aux = GameManager.GetInstance().GetProgress().categories[i].packs[j];

                    int levelsCompleted = 0;

                    for (int m = 0; m < aux.levels.Length; m++) if (aux.levels[m].completed) levelsCompleted++;

                    pack.ConfigureLevelPack(packs[j], categories[i], levelsCompleted);
                }
            }

            Color[] colors = GameManager.GetInstance().GetTheme();
            int numLetters = titleParent.transform.childCount;

            for (int i = 0; i < numLetters; i++)
            {
                Color aux = colors[i];
                aux.a = 1.0f;
                titleParent.transform.GetChild(i).GetComponent<Text>().color = aux;
            }
        }

        void Start()
        {
#if UNITY_EDITOR
            if (categoriesParent == null || categoryCardPrefab == null || levelPackCardPrefab == null || titleParent == null || adsDesactivationButton == null)
            {
                Debug.LogError("MenuManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
            LoadMainMenu();

            adsDesactivationButton.onClick.AddListener(DeactivateADS);

            if (!AdvertisingManager.GetInstance().AreADSenabled()) adsDesactivationButton.gameObject.SetActive(false); 
        }

        private void DeactivateADS()
        {
            GameManager.GetInstance().DeactivateADS();
            adsDesactivationButton.gameObject.SetActive(false);
        }

    }
}