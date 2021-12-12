using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteBackground;
        [SerializeField] SpriteRenderer spriteCircle;
        [SerializeField] SpriteRenderer upBar;
        [SerializeField] SpriteRenderer downBar;
        [SerializeField] SpriteRenderer leftBar;
        [SerializeField] SpriteRenderer rightBar;

        private int color = int.MaxValue;
        private Color tempColor; //TODO: no se si esto es temporal
        private bool isOrigin = false;
        private bool isEmpty = false;
        private List<bool> walls;


        public void Start()
        {
#if UNITY_EDITOR
           if(spriteBackground == null || spriteCircle == null || upBar == null || downBar == null ||
                leftBar == null || rightBar == null)
           {
                Debug.LogError("Tile: Alguna variable no tiene valor asociado desde el editor.");
                return;
           }
#endif
            tempColor = Color.white;

            spriteCircle.color = tempColor;
            upBar.color = tempColor;
            downBar.color = tempColor;
            leftBar.color = tempColor;
            rightBar.color = tempColor;

            //Lista de muros
            walls = new List<bool>();
            for(int i = 0; i < (int)Direction.None; ++i) {
                walls.Add(false);
            }
        }

        public void ResetData()
        {
            ResetBars();
            if (!isOrigin)
            {
                spriteCircle.enabled = false;
                color = int.MaxValue;
            }
        }

        private void ResetBars()
        {
            upBar.enabled = false;
            downBar.enabled = false;
            leftBar.enabled = false;
            rightBar.enabled = false;
        }

        private SpriteRenderer GetDirectionSprite(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return upBar;
                case Direction.Down:
                    return downBar;
                case Direction.Right:
                    return rightBar;
                case Direction.Left:
                    return leftBar;
                default:
                    //Nunca se debería llegar aquí
                    return upBar;
            }
        }

        public void SetDirection(Direction dir)
        {
            SpriteRenderer sprite = GetDirectionSprite(dir);
            sprite.enabled = true;
            sprite.material.color = tempColor;
        }

        public void ClearDirection(Direction dir)
        {
            SpriteRenderer sprite = GetDirectionSprite(dir);
            sprite.enabled = false;
        }

    //Setters
    //----------------------------------------------

        public void SetColor(int c)
        {
            //spriteCircle.material.color = c;
            color = c;
        }

        public void SetTempColor(Color c) //TODO: ESTE TIENE QUE REEMPLAZAR AL DE ARRIBA
        {
            tempColor = c;
        }

        public void SetAsOrigin(int c)
        {
            isOrigin = true;
            spriteCircle.enabled = true;
            color = c;
        }

        public void SetWall(Direction dir, bool isWall)
        {
            walls[(int)dir] = isWall;
        }

    //Getters
    //---------------------------------------------

        public bool IsOrigin()
        {
            return isOrigin;
        }

        public int GetColor()
        {
            return color;
        }

        public Color GetTempColor()
        {
            return tempColor;
        }

        public bool IsActive()
        {
            return color != int.MaxValue;
        }
        
        public void SetEmpty()
        {
            isEmpty = true;
        }

        public bool IsEmpty()
        {
            return isEmpty;
        }

        public bool IsWall(Direction dir)
        {
            return walls[(int)dir];
        }
    }
}