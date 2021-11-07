package com.example.engine;

import java.awt.Color;

public abstract class AbstractGraphics implements Graphics {

    protected float _aspect = 2f/3f; //TODO asignar el valor desde logic o desde main para que sea cosa solo del juego
    protected int _logicWidth ;
    protected int _logicHeight;
    protected int _logicOffsetX;
    protected int _logicOffsetY;

    public void setLogicCoords(){
        //Calculo de coordenadar logicas a reales
        float windowAspect = (float)getWidth() / (float)getHeight();
        if(_aspect > windowAspect) {
            _logicWidth = getWidth();
            _logicHeight = (int)(getWidth() / _aspect);
            _logicOffsetX = 0;
            _logicOffsetY = getHeight()/2 - _logicHeight/2;
        }
        else if(_aspect < windowAspect) {
            _logicWidth = (int)(getHeight() * _aspect);
            _logicHeight = getHeight();
            _logicOffsetX = getWidth()/2 - _logicWidth/2;
            _logicOffsetY = 0;
        }
    }
}
