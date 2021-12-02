using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteBackground;
    [SerializeField] SpriteRenderer spriteCircle;
    [SerializeField] SpriteRenderer upBar;
    [SerializeField] SpriteRenderer downBar;
    [SerializeField] SpriteRenderer leftBar;
    [SerializeField] SpriteRenderer rightBar;

    private Color color;
    private bool isOrigin = false;

    public void Start()
    {
        color = Color.white;

        spriteCircle.color = color;
        upBar.color = color;
        downBar.color = color;
        leftBar.color = color;
        rightBar.color = color;
    }

    public void SetColor(Color c)
    {
        //spriteCircle.color = c;
        //color = c;
    }

    public void SetAsOrigin()
    {
        isOrigin = true;
        spriteCircle.enabled = true;
    }

    public bool IsOrigin()
    {
        return isOrigin;
    }
}
