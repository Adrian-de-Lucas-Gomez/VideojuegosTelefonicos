using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class PointerIndicator
    {
        private SpriteRenderer sprite;
        private Color color;
        private bool darkened = true;

        public void SetSprite(SpriteRenderer s)
        {
            sprite = s;
        }

        public void SetPosition(Vector3 pos)
        {
            sprite.transform.position = new Vector3(pos.x, pos.y, 1);
        }

        public void Show(Color c)
        {
            sprite.enabled = true;
            color = new Color(c.r, c.g, c.b, 0.5f);
            sprite.color = color;
        }

        public void Hide()
        {
            sprite.enabled = false;
        }

        public void SetDarkened(bool dark)
        {
            darkened = dark;
            if (dark)
            {
                Color tempColor = color * 0.7f;
                sprite.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.5f);
            }
            else sprite.color = color;
        }

        public bool isDarkened() { return darkened; }
    }
}

