using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class GoMainMenuButton : MonoBehaviour
    {
        public void OnPressed()
        {
            GameManager gMng = GameManager.GetInstance();
            gMng.ChangeScene("MenuScene");
        }
    }
}