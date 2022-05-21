using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    public class LevelPage : MonoBehaviour
    {
        //Para UI
        [SerializeField] Text pageTitle;
        [SerializeField] Transform buttonsParent;

        private int pageIndex;
        private LevelPack pack;

        void Start()
        {
#if UNITY_EDITOR
            if (pageTitle == null || buttonsParent == null)
            {
                Debug.LogError("LevelPage: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
        }

        private void Configure()
        {
            if (pack == null) return;

            pageTitle.text = pack.groupTitles[pageIndex];
        }

        public void ConfigureLevelPage(int id, LevelPack pack)
        {
            this.pack = pack;
            pageIndex = id;
            Configure();
        }

        public Transform GetButtonsParent()
        {
            return buttonsParent;
        }
    }
}
