using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        FixedRows,
        FixedColumns
    }

    public FitType constraint;
    public int rows;
    public int columns;

    public float cellAspect;
    public Vector2 spacing; //Espacio entre elementos del grid

    private Vector2 cellSize;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if(constraint == FitType.Uniform)
        {
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }
        else if(constraint == FitType.FixedColumns)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        else if (constraint == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        //Calcular nuevo size de los elementos
        if(cellAspect >= 1)
        {
            float aux = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns - 1))
                        - (padding.left / (float)columns) - (padding.right / (float)columns);
            cellSize.x = aux;
            cellSize.y = aux * cellAspect;
        }
        else
        {
            float aux = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * (rows - 1))
                        - (padding.top / (float)rows) - (padding.bottom / (float)rows);
            cellSize.x = aux * cellAspect;
            cellSize.y = aux;
        }

        //Recolocar los elementos del grid
        int rowCount = 0;
        int columnCount = 0;

        for(int i = 0; i < rectChildren.Count; ++i)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var child = rectChildren[i];
            //Nueva pos x e y
            var posX = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var posY = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(child, 0, posX, cellSize.x);
            SetChildAlongAxis(child, 1, posY, cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        
    }

    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }
}
