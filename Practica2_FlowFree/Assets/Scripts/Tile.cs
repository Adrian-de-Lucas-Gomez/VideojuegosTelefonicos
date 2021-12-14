using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteBackground;
        [SerializeField] SpriteRenderer bigCircle;
        [SerializeField] SpriteRenderer smallCircle;
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
           if(spriteBackground == null || bigCircle == null || upBar == null || downBar == null ||
                leftBar == null || rightBar == null || smallCircle == null)
           {
                Debug.LogError("Tile: Alguna variable no tiene valor asociado desde el editor.");
                return;
           }
#endif
            tempColor = Color.white;
            tempColor.a = 1;

            //bigCircle.color = tempColor;
            //upBar.color = tempColor;
            //downBar.color = tempColor;
            //leftBar.color = tempColor;
            //rightBar.color = tempColor;
            //smallCircle.color = tempColor;

            //Lista de muros
            walls = new List<bool>();
            for(int i = 0; i < (int)Direction.None; ++i) {
                walls.Add(false);
            }
        }

        public void ResetData()
        {
            ResetBars();
            EnableSmallCircle(false);
            if (!isOrigin)
            {
                bigCircle.enabled = false;
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

        public void SetAsOrigin(int c)
        {
            isOrigin = true;
            bigCircle.enabled = true;
            color = c;
        }

        public void EnableSmallCircle(bool visible)
        {
            smallCircle.enabled = visible;
        }

        public bool CanBeAccesed(Direction dir)
        {
            return !walls[(int)dir] && !isEmpty;
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
            //sprite.color = tempColor;
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
            //bigCircle.material.color = c;
            color = c;
        }

        public void SetTempColor(Color c) //TODO: ESTE TIENE QUE REEMPLAZAR AL DE ARRIBA
        {
            c.a = 1;
            bigCircle.color = c;
            upBar.material.color = c;
            downBar.material.color = c;
            leftBar.material.color = c;
            rightBar.material.color = c;
            smallCircle.material.color = c;

            tempColor = c;
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