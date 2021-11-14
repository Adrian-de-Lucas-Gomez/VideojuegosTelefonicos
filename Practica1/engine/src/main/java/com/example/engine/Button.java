package com.example.engine;

public class Button {
    private Image _img = null;
    private float _x, _y, _height, _width;
    private State currentState = State.NotPressed;
    private float _alpha, _scale;
    private float _defaultAlpha, _defaultScale;

    //Animaciones
    private float _iAlpha, _iScale, _eScale;
    private float _stepAlpha;

    private boolean _hasScalingAnimation = false;
    private boolean _hasAlphaAnimation = false;

    private boolean _scalingAnimFinished = false;
    private boolean _alphaAnimationFinished = false;

    private float currentAlphaAnimationTime = 0f;
    private float currentScalingAnimationTime = 0f;

    //Tiempo por cada repetición de la animación de escalado.
    private float timePerScalingAnimationRepetition = 0f;

    private float _scalingAnimationTime = 0f;
    private float _alphaAnimationTime = 0f;

    enum State {NotPressed, Transitioning}


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

    /**
     * Devuelve un bool que indica si el usuario ha pulsado el botón con las coordenadas lógicas indicadas.
     * @param x posición X de la pulsación del jugador en cooredenadas lógicas
     * @param y posición Y de la pulsación del jugador en cooredenadas lógicas
     */
    public boolean isPressed(int x, int y){
        return(_x <= x && _y <= y && _x + _width >= x && _y + _height >= y);
    }

    /**
     * Actualiza los valores escala/alpha de sus respectivas animaciones
     * @param deltaTime tiempo desde el último frame
     */
    public void update(double deltaTime){
        if(currentState == State.Transitioning) {
            if(_hasAlphaAnimation && !_alphaAnimationFinished) {
                currentAlphaAnimationTime += deltaTime;
                _alpha += _stepAlpha * deltaTime;
                if(currentAlphaAnimationTime > _alphaAnimationTime) _alphaAnimationFinished = true;
            }
            if(_hasScalingAnimation && !_scalingAnimFinished) {
                currentScalingAnimationTime += deltaTime;
                _scale = (_eScale - _iScale) * (float)Math.sin(currentScalingAnimationTime * (2f* Math.PI) / timePerScalingAnimationRepetition) + _iScale;
                if(currentScalingAnimationTime > _scalingAnimationTime) _scalingAnimFinished = true;
            }
            if((!_hasScalingAnimation || _scalingAnimFinished) && (!_hasAlphaAnimation|| _alphaAnimationFinished)){
                currentState = State.NotPressed;
                _scale = _defaultScale;
                _alpha = _defaultAlpha;
            }
        }
    }
    /**
     * Activa flags de las animaciones. Estas se actualizan en update()
     */
    public void activateAnimation(){
        if(_hasAlphaAnimation){
            currentAlphaAnimationTime = 0f;
            _alphaAnimationFinished = false;
            _alpha = _iAlpha;
        }
        if(_hasScalingAnimation){
            currentScalingAnimationTime = 0f;
            _scalingAnimFinished = false;
            _scale = _iScale;
        }
        currentState = State.Transitioning;
    }

    /**
     * Añade una animación en la que la transparencia del botón va variando.
     * @param iAlpha valor alpha inicial
     * @param eAlpha valor alpha final
     * @param animationTime tiempo que dura la animación
     */
    public void setAnimationAlpha(float iAlpha, float eAlpha, float animationTime){
        _hasAlphaAnimation = true;
        _iAlpha = iAlpha;
        _alphaAnimationTime = animationTime;

        _stepAlpha = (eAlpha - iAlpha) / _alphaAnimationTime;
    }

    /**
     * Añade una animación en la que la escala del botón va variando.
     * @param iScale valor de escala inicial
     * @param eScale valor de escala final
     * @param animationTime tiempo que dura la animación
     * @param nReps número de veces que se repite la animación en el tiempo determinado. Con esto
     *              se consigue el efecto de "bote" de los muros que no se pueden modificar.
     */
    public void setScalingAnimation(float iScale, float eScale, float animationTime, int nReps){
        _hasScalingAnimation = true;
        _iScale = iScale;
        _eScale = eScale;
        _scalingAnimationTime = animationTime;

        timePerScalingAnimationRepetition = animationTime / nReps;
    }

    public float getX() { return _x; }

    public float getY() { return _y; }

    public float getWidth() { return _width; }

    public float getHeight() { return _height; }

    public float getAlpha() {return _alpha;}

    public float getScale() {return _scale;}

    public Image getImage() {return _img;}
}
