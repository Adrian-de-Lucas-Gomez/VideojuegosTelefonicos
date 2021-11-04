package com.example.engine;
import com.example.engine.Graphics;

public abstract class AbstractGraphics implements Graphics {
    void setCanvas(){
            //Miramos tamaño de la pantalla para saber como adaptarla???
        }

    /*  Supongo que estps metodos si no los voy a implementar
        no es necesario que los nombre en la clase abstracta

    abstract Image newImage(String name);
    abstract Font newFont(String filename, int size, boolean isBold);
    abstract void clear(int color);
    abstract void translate(int x, int y);
    abstract void scale(int x, int y);
    abstract void save();
    abstract void restore();
    abstract void drawImage(Image image);
    abstract void setColor(Color color);
    abstract void fillCircle(int cx, int cy, int r);
    abstract void drawText(String text, int x, int y);
    abstract int getWidth();
    abstract int getHeight();
    */
}
