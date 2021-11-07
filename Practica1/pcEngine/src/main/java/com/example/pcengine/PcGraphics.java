package com.example.pcengine;

import com.example.engine.AbstractGraphics;
import com.example.engine.Font;
import com.example.engine.Image;

import java.awt.Color;
import java.awt.Graphics2D;
import java.io.IOException;

//Ahora implementan la clase abstracta AbstractGraphics

public class PcGraphics extends AbstractGraphics {

    private Window _window;
    //private java.awt.image.BufferStrategy _strategy;
    private java.awt.Graphics2D _graphics;

    public PcGraphics(Window window){
        _window = window;
        //_strategy =  _window.getBufferStrategy();
    }

    public Image newImage(String name){
        PcImage img = null;
        try {
            img = new PcImage(javax.imageio.ImageIO.read(new java.io.File("./assets/sprites/" + name)));
        } catch (IOException e) {
            System.err.println("Couldn't load image: " + name);
            e.printStackTrace();
        }
        return img;
    }

    public PcFont newFont(String filename, float size, boolean isBold){
        int style;
        if(isBold){
            style = java.awt.Font.BOLD;
        }
        else{
            style = java.awt.Font.PLAIN;
        }
        return new PcFont(filename, style, size);
    }

    public void clear(int color){
        _graphics.setColor(new Color(color));
        _graphics.fillRect(0, 0, _window.getWidth(), _window.getHeight());
    }

    public void translate(int x, int y){

    }

    public void scale(int x, int y){

    }

    public void save(){

    }

    public void restore(){

    }

    //TODO igual hace falta uno con alpha en algun momento
    public void drawImage(Image image, int x, int y, int w, int h){
        if(image != null) {
            int realX = x + _logicOffsetX;
            int realY = y + _logicOffsetY;
             _graphics.drawImage(((PcImage)image).getImage(), realX, realY, w, h, null);
        }
        else
            System.out.println("Null image :(");
    }

    public void setColor(Color color){
        _graphics.setColor(color);
    }

    public void fillCircle(int cx, int cy, int r){
        int realX = cx + _logicOffsetX;
        int realY = cy + _logicOffsetY;
        _graphics.fillOval(realX - r, realY - r, 2*r, 2*r);
    }

    public void drawText(Font font, String text, int x, int y){
        _graphics.setFont(((PcFont)font).getFont());
        _graphics.drawString(text, x, y);
    }

    public int getWidth(){
        return _window.getWidth();
    }

    public int getHeight() {
        return _window.getHeight();
    }

    public void setGraphics(Graphics2D g){
        _graphics = g;
    }
}