package com.example.engine;

public class Button {
    private Image _img = null;
    private float _x, _y, _height, _width;
    private State currentState = State.NotPressed;
    private float alpha = 1.0f, scale = 1.0f;

    //Animaciones
    private float _iAlpha, _eAlpha, _iScale, _eScale;
    private float _timeForAnim;
    private float _stepAlpha, _stepScale;

    enum State {NotPressed, Pressed, Transitioning}

    public Button(float posX, float posY, float width, float height, Image img){
        _img = img;
        _x = posX;
        _y = posY;
        _width = width;
        _height = height;
    }

    public State getState() {return currentState;}

    public boolean isPressed(int x, int y){
        return(_x <= x && _y <= y && _x + _width >= x && _y + _height >= y);
    }

    public void step(double deltaTime){
        if(currentState == State.Transitioning) {
            alpha += _stepAlpha * deltaTime;
            scale += _stepScale * deltaTime;

            if(alpha > _eAlpha || _stepScale > _eScale) {
                currentState = State.NotPressed;
                scale = _eScale;
                alpha = _eAlpha;
            }
        }
    }

    public void activateAnimation(){
        currentState = State.Transitioning;
        scale = _iScale;
        alpha = _iAlpha;
    }

    public void setAnimParams(float iAlpha, float eAlpha, float iScale, float eScale, float animationTime){
        _iAlpha = iAlpha;
        _eAlpha = eAlpha;
        _iScale = iScale;
        _eScale = eScale;
        _timeForAnim = animationTime;

        _stepAlpha = (eAlpha - iAlpha) / animationTime;
        _stepScale = (eScale - iScale) / animationTime;
    }

    public float getX() { return _x; }

    public float getY() { return _y; }

    public float getWidth() { return _width; }

    public float getHeight() { return _height; }

    public float getAlpha() {return alpha;}

    public float getScale() {return scale;}

    public Image getImage() {return _img;}
}
