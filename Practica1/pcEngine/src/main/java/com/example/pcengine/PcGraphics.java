package com.example.pcengine;

import com.example.engine.AbstractGraphics;
import com.example.engine.Font;
import com.example.engine.Image;

import java.awt.AlphaComposite;
import java.awt.Color;
import java.awt.Composite;
import java.awt.FontMetrics;
import java.awt.Graphics2D;
import java.awt.geom.AffineTransform;
import java.io.IOException;
import javax.swing.Timer;

//Ahora implementan la clase abstracta AbstractGraphics

public class PcGraphics extends AbstractGraphics {

    private Window _window;
    private java.awt.Graphics2D _graphics;
    private AffineTransform _tr;
    private FontMetrics fontMetrics;
    private Composite oComposite;
    private Composite aComposite;

    public PcGraphics(Window window, int width, int height){
        _window = window;
        System.out.println(width);
        System.out.println(height);
        _gameWidth = width;
        _gameHeight = height;
        _aspect = (float)_gameWidth / (float)_gameHeight;
        oComposite = AlphaComposite.getInstance(AlphaComposite.SRC_OVER, 1f);
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
    public void drawImage(Image image, float w, float h, float alpha){
        if(image != null) {
            if(alpha < 1f){
                aComposite = AlphaComposite.getInstance(AlphaComposite.SRC_OVER, alpha);
                _graphics.setComposite(aComposite);
            }
            _graphics.drawImage(((PcImage)image).getImage(), 0, 0, (int)w, (int)h, null);
            _graphics.setComposite(oComposite);
        }
        else
            System.out.println("Null image :(");
    }

    public void setColor(int color){
        _graphics.setColor(new Color(color));
    }

    public void setFont(Font font) {
        _graphics.setFont(((PcFont)font).getFont());
        fontMetrics = _graphics.getFontMetrics(((PcFont)font).getFont());
    }

    public void fillCircle(float cx, float cy, float r, float alpha){ //TODO quitar cx y cy
        aComposite = AlphaComposite.getInstance(AlphaComposite.SRC_OVER, alpha);
        _graphics.setComposite(aComposite);
        _graphics.fillOval((int)(-r + cx), (int)(-r + cy), (int)(r*2), (int)(r*2));
        _graphics.setComposite(oComposite);
    }

    public void drawText(String text, float x, float y, Boolean isCenteredX, Boolean isCenteredY){
        if(isCenteredX) x -= getTextWidth(text) * 0.5;
        if(isCenteredY) y += getTextHeight(text) * 0.5;
        _graphics.drawString(text, x, y);
    }

    @Override
    public int getTextWidth(String text){ ;
        return (int)fontMetrics.getStringBounds(text, _graphics).getWidth();
    }

    @Override
    public int getTextHeight(String text){
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