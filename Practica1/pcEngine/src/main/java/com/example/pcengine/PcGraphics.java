package com.example.pcengine;

import com.example.engine.AbstractGraphics;
import com.example.engine.Font;
import com.example.engine.Image;

import java.awt.Color;
import java.awt.FontMetrics;
import java.awt.Graphics2D;
import java.awt.geom.AffineTransform;
import java.io.IOException;

//Ahora implementan la clase abstracta AbstractGraphics

public class PcGraphics extends AbstractGraphics {

    private Window _window;
    private java.awt.Graphics2D _graphics;
    private AffineTransform _tr;

    public PcGraphics(Window window, int width, int height){
        _window = window;
        System.out.println(width);
        System.out.println(height);
        _gameWidth = width;
        _gameHeight = height;
        _aspect = (float)_gameWidth / (float)_gameHeight;
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

    public void translate(float x, float y){
        _graphics.translate(x, y);
    }

    public void scale(float x, float y){
        _graphics.scale(x, y);
    }

    public void save(){ //TODO
        _tr = _graphics.getTransform();
    }

    public void restore(){ //TODO
        _graphics.setTransform(_tr);
    }

    //TODO igual hace falta uno con alpha en algun momento
    public void drawImage(Image image, float w, float h){
        if(image != null) {
            _graphics.drawImage(((PcImage)image).getImage(), 0, 0, (int)w, (int)h, null);
        }
        else
            System.out.println("Null image :(");
    }

    public void setColor(Color color){
        _graphics.setColor(color);
    }

    public void fillCircle(float cx, float cy, float r){ //TODO quitar cx y cy
        _graphics.fillOval((int)(-r + cx), (int)(-r + cy), (int)(r*2), (int)(r*2));
    }

    public void drawText(Font font, String text, float x, float y){ //TODO quitar x e y
        _graphics.setFont(((PcFont)font).getFont());
        _graphics.drawString(text, x, y);
    }

    @Override
    public int getTextWidth(Font font, String text){
        FontMetrics fontMetrics = _graphics.getFontMetrics(((PcFont)font).getFont());
        //_graphics.draw(fontMetrics.getStringBounds(text, _graphics));
        return (int)fontMetrics.getStringBounds(text, _graphics).getWidth();
    }

    @Override
    public int getTextHeight(Font font, String text){
        FontMetrics fontMetrics = _graphics.getFontMetrics(((PcFont)font).getFont());
        //_graphics.draw(fontMetrics.getStringBounds(text, _graphics));
        return (int)fontMetrics.getStringBounds(text, _graphics).getHeight();
    }

    public int getWindowWidth(){
        return _window.getWidth();
    }

    public int getWindowHeight() {
        return _window.getHeight();
    }

    public void setGraphics(Graphics2D g){
        _graphics = g;
    }
}