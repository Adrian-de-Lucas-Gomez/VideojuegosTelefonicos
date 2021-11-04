package com.example.pcengine;

import com.example.engine.AbstractGraphics;
import com.example.engine.Image;

import java.awt.Color;
//Ahora implementan la clase abstracta AbstractGraphics

public class PcGraphics extends AbstractGraphics {
    public PcImage newImage(String name){
        //Deberiamos de crear un objeto imagen aqui
        return new PcImage(/*imagen*/);
    }
    public PcFont newFont(String filename, int size, boolean isBold){
        return new PcFont();
    }
    public void clear(int color){}
    public void translate(int x, int y){}
    public void scale(int x, int y){}
    public void save(){}
    public void restore(){}
    public void drawImage(Image /*Lol*/ image /*mas parametros?*/){}    //Revisar
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