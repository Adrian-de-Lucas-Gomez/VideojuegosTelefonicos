package com.example.pcengine;

import com.example.engine.AbstractGraphics;
import com.example.engine.Font;
import com.example.engine.Image;

import java.awt.Color;
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

    public void translate(int x, int y){
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
    public void drawImage(Image image, int w, int h){
        if(image != null) {
            _graphics.drawImage(((PcImage)image).getImage(), 0, 0, w, h, null);
        }
        else
            System.out.println("Null image :(");
    }

    public void setColor(Color color){
        _graphics.setColor(color);
    }

    public void fillCircle(int cx, int cy, int r){ //TODO quitar cx y cy
        _graphics.fillOval(0 - r, 0 - r, 2*r, 2*r);
    }

    public void drawText(Font font, String text, int x, int y){ //TODO quitar x e y
        System.out.println(_graphics.getFontRenderContext().getTransform().getScaleX());
        _graphics.setFont(((PcFont)font).getFont().deriveFont(AffineTransform.getScaleInstance(_scaleAspect, _scaleAspect)));
        _graphics.drawString(text, x, y);
    }

    /*public float getScreenWidthText(Font font, String text){
        FontMetrics fontMetrics = _graphics.getFontMetrics(font);
        return fontMetrics.stringWidth(text);
    }

    public float getScreenHeightText(Font font, String text){
        FontMetrics fontMetrics = _graphics.getFontMetrics(font);
        return fontMetrics.stringWidth(text);
    }*/

    public int getWindowWidth(){
        return _window.getWidth();
    }

    public int getWindowHeight() {
        return _window.getHeight();
    }

    @Override
    public int getTextHeight(Font font, String string) {
        java.awt.Font aux = ((PcFont)font).getFont().deriveFont(((PcFont) font).getFont().getSize() * _scaleAspect);
        return _graphics.getFontMetrics(aux).stringWidth(string);
    }

    public void setGraphics(Graphics2D g){
        _graphics = g;
    }
}