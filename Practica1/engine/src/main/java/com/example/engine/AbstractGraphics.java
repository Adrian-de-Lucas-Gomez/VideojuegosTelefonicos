package com.example.engine;

public abstract class AbstractGraphics implements Graphics {

    protected int _gameWidth;
    protected int _gameHeight;
    protected float _aspect = 1.0f;
    protected float _scaleAspect = 1.0f;
    protected int _offsetX = 0;
    protected int _offsetY = 0;

    public void setScaleAspect(){
        float logicAspect = (float)getWindowWidth() / (float)getWindowHeight();

        if(_aspect > logicAspect) {
            _offsetX = 0;
            _offsetY = (int)((float)getWindowHeight()/2 - ((float)getWindowWidth()/_aspect)/2);
            _scaleAspect = (float)(getWindowWidth()/_aspect) / (float)_gameHeight;
        }
        else if(_aspect < logicAspect) {
            _offsetX = (int)((float)getWindowWidth()/2 - (float)(getWindowHeight() * _aspect)/2);
            _offsetY = 0;
            _scaleAspect = (float)(getWindowHeight() * _aspect) / (float)_gameWidth;
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
