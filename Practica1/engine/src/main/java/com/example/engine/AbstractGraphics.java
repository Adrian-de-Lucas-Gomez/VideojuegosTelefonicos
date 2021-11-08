package com.example.engine;

import java.awt.Color;

public abstract class AbstractGraphics implements Graphics {

    protected int _gameWidth;
    protected int _gameHeight;
    protected float _aspect;

    protected int _logicOffsetX;
    protected int _logicOffsetY;
    protected float _logicScaleAspect;

    public void setLogicCoords(){
        //Calculo de coordenadas logicas a reales
        float logicAspect = (float)getWidth() / (float)getHeight();

        if(_aspect > logicAspect) {
            _logicOffsetX = 0;
            _logicOffsetY = (int)((float)getHeight()/2 - ((float)getWidth()/_aspect)/2);
            _logicScaleAspect = (float)(getWidth()/_aspect) / (float)_gameHeight;
        }
        else if(_aspect < logicAspect) {
            _logicOffsetX = (int)((float)getWidth()/2 - (float)(getHeight() * _aspect)/2);
            _logicOffsetY = 0;
            _logicScaleAspect = (float)(getHeight() * _aspect) / (float)_gameWidth;
        }
    }

    public int getGameWidth() {
        return _gameWidth;
    }

    public int getGameHeight() {
        return _gameHeight;
    }
    
    public int getLogicOffsetX() {
        return _logicOffsetX;
    }

    public int getLogicOffsetY() {
        return _logicOffsetY;
    }
}
