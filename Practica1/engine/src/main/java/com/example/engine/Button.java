package com.example.engine;

import sun.util.resources.ext.CalendarData_en_IE;

public class Button {
    private Image _img = null;
    private float _x, _y, _height, _width;
    private State currentState = State.NotPressed;
    private float _alpha = 1.0f, _scale = 1.0f;
    private float _defaultAlpha, _defaultScale;

    //Animaciones
    private float _iAlpha, _eAlpha, _iScale, _eScale;
    private float _stepAlpha, _stepScale;

    private boolean _hasScalingAnimation = false;
    private boolean _hasAlphaAnimation = false;

    private boolean _scalingAnimFinished = false;
    private boolean _alphaAnimationFinished = false;

    enum State {NotPressed, Pressed, Transitioning}

    public Button(float posX, float posY, float width, float height, Image img, float alpha, float scale){
        _img = img;
        _x = posX;
        _y = posY;
        _width = width;
        _height = height;

        _alpha = alpha;
        _scale = scale;

        _defaultAlpha = alpha;
        _defaultScale = scale;
    }

    public State getState() {return currentState;}

    public boolean isPressed(int x, int y){
        return(_x <= x && _y <= y && _x + _width >= x && _y + _height >= y);
    }

    public void step(double deltaTime){
        if(currentState == State.Transitioning) {
            if(_hasAlphaAnimation && !_alphaAnimationFinished) {
                _alpha += _stepAlpha * deltaTime;
                if(_alpha < Math.min(_iAlpha, _eAlpha) || _alpha > Math.max(_iAlpha, _eAlpha)) _alphaAnimationFinished = true;
            }
            if(_hasScalingAnimation && !_scalingAnimFinished) {
                _scale += _stepScale * deltaTime;
                if(_scale < Math.min(_iScale, _eScale) || _scale > Math.max(_iScale, _eScale)) _scalingAnimFinished = true;
            }

            if((!_hasScalingAnimation || _scalingAnimFinished) && (!_hasAlphaAnimation|| _alphaAnimationFinished)){
                currentState = State.NotPressed;
                _scale = _defaultScale;
                _alpha = _defaultAlpha;
            }
        }
    }

    public void activateAnimation(){
        if(_hasAlphaAnimation){
            _alphaAnimationFinished = false;
            _alpha = _iAlpha;
        }
        if(_hasScalingAnimation){
            _scalingAnimFinished = false;
            _scale = _iScale;
        }
        currentState = State.Transitioning;
    }

    public void setAnimationAlpha(float iAlpha, float eAlpha, float animationTime){
        _hasAlphaAnimation = true;
        _iAlpha = iAlpha;
        _eAlpha = eAlpha;

        _stepAlpha = (eAlpha - iAlpha) / animationTime;
    }

    public void setScalingAnimation(float iScale, float eScale, float animationTime){
        _hasScalingAnimation = true;
        _iScale = iScale;
        _eScale = eScale;

        _stepScale = (eScale - iScale) / animationTime;
    }

    public float getX() { return _x; }

    public float getY() { return _y; }

    public float getWidth() { return _width; }

    public float getHeight() { return _height; }

    public float getAlpha() {return _alpha;}

    public float getScale() {return _scale;}

    public Image getImage() {return _img;}
}
