using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteBackground;
        [SerializeField] SpriteRenderer outline;
        [SerializeField] SpriteRenderer bigCircle;
        [SerializeField] SpriteRenderer smallCircle;
        [SerializeField] SpriteRenderer upBar;
        [SerializeField] SpriteRenderer downBar;
        [SerializeField] SpriteRenderer leftBar;
        [SerializeField] SpriteRenderer rightBar;
        [SerializeField] SpriteRenderer transparentBackground;
        [SerializeField] SpriteRenderer hintMarker;
        [SerializeField] SpriteRenderer fadingCircle;
        [SerializeField] SpriteRenderer upWall;
        [SerializeField] SpriteRenderer downWall;
        [SerializeField] SpriteRenderer leftWall;
        [SerializeField] SpriteRenderer rightWall;

        [SerializeField] Animator smallCircleAnimator;
        [SerializeField] Animator bigCircleAnimator;
        [SerializeField] Animator fadingCircleAnimator;

        private int color = int.MaxValue;
        private Color renderColor;
        private bool isOrigin = false;
        private bool isEmpty = false;
        private List<SpriteRenderer> wallsSprites;
        private List<bool> walls;


        public void Start()
        {
#if UNITY_EDITOR
           if(spriteBackground == null || bigCircle == null || upBar == null || downBar == null ||
                leftBar == null || rightBar == null || smallCircle == null || transparentBackground == null || hintMarker == null
                || smallCircleAnimator == null || bigCircleAnimator == null || fadingCircle == null || fadingCircleAnimator == null
                || upWall == null || downWall == null || leftWall == null || rightWall == null || outline == null)
           {
                Debug.LogError("Tile: Alguna variable no tiene valor asociado desde el editor.");
                return;
           }
#endif
        }

        public void Initialize(Color wallColor)
        {
            renderColor = Color.white;
            renderColor.a = 1;

            wallColor.a = 1;

            //Lista de muros
            walls = new List<bool>();
            for (int i = 0; i < (int)Direction.None; ++i)
            {
                walls.Add(false);
            }
            wallsSprites = new List<SpriteRenderer>();
            upWall.color = wallColor;
            wallsSprites.Add(upWall);
            downWall.color = wallColor;
            wallsSprites.Add(downWall);
            leftWall.color = wallColor;
            wallsSprites.Add(leftWall);
            rightWall.color = wallColor;
            wallsSprites.Add(rightWall);
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
            if (visible) smallCircleAnimator.Play("Trigger");
        }

        public void HideTransparentBackground()
        {
            transparentBackground.enabled = false;
        }

        public void ShowTransparentBackground(Color c)
        {
            transparentBackground.enabled = true;
            transparentBackground.color = new Color(c.r, c.g, c.b, 0.2f);
        }

        public bool CanBeAccesed(Direction dir)
        {
            return !walls[(int)dir] && !isEmpty;
        }

        public void setHintMarker(bool enable)
        {
            hintMarker.enabled = enable;
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

        public void PlayBigCircleAnimation()
        {
            bigCircleAnimator.Play("Trigger");
        }

        public void PlayFadingCircleAnimation()
        {
            fadingCircleAnimator.Play("Trigger");
        }

    //Setters
    //----------------------------------------------

        public void SetColor(int c)
        {
            color = c;
        }

        public void SetRenderColor(Color c)
        {
            c.a = 1;
            bigCircle.color = c;
            upBar.material.color = c;
            downBar.material.color = c;
            leftBar.material.color = c;
            rightBar.material.color = c;
            smallCircle.material.color = c;
            fadingCircle.material.color = c;

            renderColor = c;
        }

        public void SetWall(Direction dir, bool isWall)
        {
            walls[(int)dir] = isWall;
            wallsSprites[(int)dir].enabled = isWall;
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
            return renderColor;
        }

        public bool IsActive()
        {
            return color != int.MaxValue;
        }
        
        public void SetEmpty()
        {
            isEmpty = true;
            outline.enabled = false;
            spriteBackground.enabled = false;
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