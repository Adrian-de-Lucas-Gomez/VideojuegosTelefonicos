using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class SkinManager : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        private bool panelActive = false;

        private void Start()
        {
            panel.SetActive(panelActive);
        }

        public void onClick()
        {
            panelActive = !panelActive;
            panel.SetActive(panelActive);
        }
    }
}