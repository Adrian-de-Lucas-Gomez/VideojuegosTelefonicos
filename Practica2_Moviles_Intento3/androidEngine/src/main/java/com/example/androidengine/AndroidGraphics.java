package com.example.androidengine;

import com.example.engine.Graphics;
import com.example.engine.Image;


import java.awt.Color;



public class AndroidGraphics implements Graphics {
    public AndroidImage newImage(String name){
        return new AndroidImage(0, 0);
    }
    public AndroidFont newFont(String filename, int size, boolean isBold){
        return new AndroidFont();
    }
    public void clear(int color){}
    public void translate(int x, int y){}
    public void scale(int x, int y){}
    public void save(){}
    public void restore(){}
    public void drawImage(Image /*Lol*/ image /*mas parametros?*/){}
    public void setColor(Color color){}
    public void fillCircle(int cx, int cy, int r){}
    public void drawText(String text, int x, int y){}
    public int getWidth(){
        return width;
    }
    public int getHeight() {
        return height;
    }

    private int width = 0;
    private int height = 0;
}
