package com.example.pcengine;

import com.example.engine.AbstractGraphics;
import com.example.engine.Image;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.image.BufferStrategy;
import java.io.IOException;

//import sun.jvm.hotspot.code.PCDesc;
//Ahora implementan la clase abstracta AbstractGraphics

public class PcGraphics extends AbstractGraphics {

    private BufferStrategy _strategy;
    private Window _window;

    public PcGraphics(Window window){
        _window = window;
        _strategy = _window.getBufferStrategy();
    }

    public Image newImage(String name){
        PcImage img = null;
        try {
            img = new PcImage(javax.imageio.ImageIO.read(new java.io.File("./assets" + name)));
        } catch (IOException e) {
            System.err.println("Couldn't load image: " + name);
            e.printStackTrace();
        }
        return img;
    }

    public PcFont newFont(String filename, int size, boolean isBold){
        return new PcFont();
    }

    public void clear(int color){
        Graphics graphics = _strategy.getDrawGraphics();
        graphics.setColor(new Color(color));
        graphics.fillRect(0, 0, _window.getWidth(), _window.getHeight());
    }

    public void translate(int x, int y){}

    public void scale(int x, int y){}

    public void save(){}

    public void restore(){}

    public void drawImage(Image /*Lol*/ image /*mas parametros?*/){}    //Revisar

    public void setColor(Color color){}

    public void fillCircle(int cx, int cy, int r){}

    public void drawText(String text, int x, int y){}

    public int getWidth(){ //TODO
        return 1;
    }

    public int getHeight() {//TODO
        return 1;
    }

    /*
    // Pintamos el frame con el BufferStrategy
			do {
				do {
					Graphics graphics = strategy.getDrawGraphics();
					try {
						ventana.render(graphics);
					}
					finally {
						graphics.dispose();
					}
				} while(strategy.contentsRestored());
				strategy.show();
			} while(strategy.contentsLost());

    Copiar descaradamente
        Consejo de adri
     */
}