using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class SkinButton : MonoBehaviour
    {
        [SerializeField] int skinIndex;
        public void onClick()
        {
            GameManager.GetInstance().SetTheme(skinIndex);
        }
    }
}