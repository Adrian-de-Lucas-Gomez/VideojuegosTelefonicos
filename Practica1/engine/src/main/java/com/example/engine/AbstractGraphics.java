package com.example.engine;

import java.awt.Color;

public abstract class AbstractGraphics implements Graphics {

    protected int _gameWidth;
    protected int _gameHeight;
    protected float _aspect;
    protected float _scaleAspect;
    protected int _offsetX;
    protected int _offsetY;

    public void setScaleAspect(){
        float aspectX = (float)getWidth() / (float)_gameWidth;
        float aspectY = (float)getHeight() / (float)_gameHeight;

        if(_gameHeight * aspectX > getHeight()) {
            _scaleAspect = aspectY;
            _offsetX = (int)((float)getWidth()/2 - (float)_gameWidth/2);
            _offsetY = 0;
        }
        else {
            _scaleAspect = aspectX;
            _offsetX = 0;
            _offsetY = (int)((float)getHeight()/2 - (float)_gameHeight/2);
        }
    }

    public void setLogicCoords(){
        //Calculo de coordenadas logicas a reales
        float logicAspect = (float)getWidth() / (float)getHeight();

        if(_aspect > logicAspect) {
            _offsetX = 0;
            _offsetY = (int)((float)getHeight()/2 - ((float)getWidth()/_aspect)/2);
            _scaleAspect = (float)(getWidth()/_aspect) / (float)_gameHeight;
        }
        else if(_aspect < logicAspect) {
            _offsetX = (int)((float)getWidth()/2 - (float)(getHeight() * _aspect)/2);
            _offsetY = 0;
            _scaleAspect = (float)(getHeight() * _aspect) / (float)_gameWidth;
        }
    }


    public int getGameWidth() {
        return _gameWidth;
    }

    public int getGameHeight() {
        return _gameHeight;
    }

    public float getLogicScaleAspect() {
        return _scaleAspect;
    }

    public int getOffsetX() {
        return _offsetX;
    }

    public int getOffsetY() {
        return _offsetY;
    }
}
