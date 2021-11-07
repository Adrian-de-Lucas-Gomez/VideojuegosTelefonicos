package com.example.pcengine;

import com.example.engine.AbstractGraphics;
import com.example.engine.Image;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.image.BufferStrategy;
import java.io.File;
import java.io.IOException;

//import sun.jvm.hotspot.code.PCDesc;
//Ahora implementan la clase abstracta AbstractGraphics

public class PcGraphics extends AbstractGraphics {

    private Window _window;
    private java.awt.image.BufferStrategy _strategy;
    private java.awt.Graphics2D _graphics;

    public PcGraphics(Window window){
        _window = window;
        _strategy =  _window.getBufferStrategy();
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


    public PcFont newFont(String filename, int size, boolean isBold){
        //Font customFont = java.awt.Font.createFont(Font.TRUETYPE_FONT, new File("Fonts\\custom_font.ttf")).deriveFont(12f);
        return new PcFont("Helvetica", Font.BOLD, size);
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


    public void drawImage(Image image, int x, int y, int w, int h){
        if(image != null)
            _graphics.drawImage(((PcImage)image).getImage(), x, y, w, h, null);
        else
            System.out.println("no drawImage :("); //TODO
    }


    public void setColor(Color color){
        _graphics.setColor(color);
    }


    public void fillCircle(int cx, int cy, int r){
        _graphics.fillOval(cx - r, cy - r, 2*r, 2*r);
    }


    public void drawText(String text, int x, int y){

    }


    public int getWidth(){ //TODO
        return _window.getWidth();
    }


    public int getHeight() {//TODO
        return _window.getHeight();
    }

    public void setGraphics(Graphics2D g){
        _graphics = g;
    }
}