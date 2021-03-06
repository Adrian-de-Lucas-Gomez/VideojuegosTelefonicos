package com.example.logic;

import com.example.engine.Application;
import com.example.engine.Button;
import com.example.engine.Engine;
import com.example.engine.Font;
import com.example.engine.Graphics;
import com.example.engine.Image;
import com.example.engine.Input;
import com.example.engine.TouchEvent;

import java.util.ArrayList;

public class Logic implements Application {

    private enum GameState{
        MainMenu, BoardSizeMenu, Game
    }

    private Engine _engine;
    private Graphics _graphics;
    private Input _input;

    private int _clearColor, _black, _red, _lightgrey, _grey, _blue, _white;

    private GameState currentState;
    /*
    Hay elementos (botones, imágenes, ...) que no necesitamos generar cada vez que visitemos un nuevo estado.
    Este vector guarda los estados que hemos visitado para que cuando accedamos a uno que ya hemos visitado, como el estado de juego,
    podremos reutilizar sus elementos en vez de hacer varios new().
     */
    private boolean[] hasBeenGenerated;

    private Font _molleRegularTitle;
    private Font _josefinSansTitle;
    private Font _josefinSansText;

    //Menu state
    private Image _q43Img;
    private Button _playButton;

    //ChooseBoardSize state
    private Button _goBackButton;
    private Button[] _chooseSizeButtons;
    private boolean justSolvedBoard = false;

    //Game state
    private int boardSize = 0;
    private Board _board;
    private Image _lockImg;
    private Button _hintButton, _reverseButton;

    private float timeBetweenStates = 1.5f;
    private float sceneAlpha = 1f;

    //Transición entre estados.
    private boolean isTransitioning = false;
    private boolean hasFinishedHalfTransition = false;
    private GameState stateToTransition = GameState.MainMenu;
    private float timeToTransition = 2f;
    private float currentTimeToTransition = 0f;
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public Logic(Engine engine) {
        _engine = engine;
    }

    @Override
    public void init() {
        _graphics = _engine.getGraphics();
        _input = _engine.getInput();

        _clearColor = 0xFFFFFF;
        _black = 0x333333;
        _red = 0xE63247;
        _lightgrey = 0xEEEEEE;
        _grey = 0xAAAAB2;
        _blue = 0x60C1E0;
        _white = 0xFFFFFF;

        hasBeenGenerated = new boolean[GameState.values().length];

        _molleRegularTitle = _graphics.newFont("Molle-Regular", 100.0f, true);
        _josefinSansTitle = _graphics.newFont("JosefinSans-Bold", 50.0f, true);
        _josefinSansText = _graphics.newFont("JosefinSans-Bold", 20.0f, false);
        setState(GameState.MainMenu);
    }

    @Override
    public void onDestroy() {

    }

    @Override
    public void onHandleInput() {
        ArrayList<TouchEvent> eventList = _input.getTouchEvents();
        if(!isTransitioning || (isTransitioning && hasFinishedHalfTransition)){
            for (TouchEvent e: eventList) {
                //Ya vienen transformados desde Input
                int pointerX = e.posX;
                int pointerY = e.posY;

                if(e.eventType == TouchEvent.EventType.buttonPressed){
                    if(currentState == GameState.MainMenu){
                        if(_playButton.isPressed(pointerX, pointerY)) transitionTowardsState(GameState.BoardSizeMenu);
                    }
                    else if(currentState == GameState.BoardSizeMenu){
                        if (_goBackButton.isPressed(pointerX, pointerY)) {
                            transitionTowardsState(GameState.MainMenu);
                            _goBackButton.activateAnimation();
                        }
                        else{
                            for(int k = 0; k < _chooseSizeButtons.length; k++){
                                if(_chooseSizeButtons[k].isPressed(pointerX, pointerY)){
                                    boardSize = k + 4;
                                    justSolvedBoard = false;
                                    _chooseSizeButtons[k].activateAnimation();
                                    transitionTowardsState(GameState.Game);
                                    break;
                                }
                            }
                        }
                    }
                    else if (currentState == GameState.Game && (!_board.isSolved())){
                        _board.handleInput(pointerX, pointerY);
                        if (_goBackButton.isPressed(pointerX, pointerY)) {
                            transitionTowardsState(GameState.BoardSizeMenu);
                            _goBackButton.activateAnimation();
                            justSolvedBoard = false;
                        }
                        else if(_hintButton.isPressed(pointerX, pointerY)) {
                            _board.searchHintInTile();
                            _hintButton.activateAnimation();
                        }
                        else if(_reverseButton.isPressed(pointerX, pointerY)) {
                            _board.revertPlay();
                            _reverseButton.activateAnimation();
                        }
                        if(_board.isSolved()) {
                            transitionTowardsState(GameState.BoardSizeMenu);
                            justSolvedBoard = true;
                        }
                    }
                }
            }
        }
    }

    @Override
    public void onUpdate(double deltaTime) {
        //Si está cambiando de estado, se aplica una animación de transición en la que disminuye el alpha de toda la escena para subir de nuevo.
        if(isTransitioning){
            currentTimeToTransition += deltaTime;
            //Son mates un poco feas pero en esencia es una función seno que en un tiempo determinado recorre los valores 1 a 0 y de vuelta a 1, sin pasar por números negativos.
            sceneAlpha = (float)Math.cos((currentTimeToTransition - 2 * timeToTransition) * (2 * Math.PI) / 2) * 0.5f + 0.5f;
            _graphics.setMaxAlpha(sceneAlpha);

            //Si la animación va por la mitad (alpha es 0), se genera el nuevo estado. De este modo la transición es lo más fluida posible
            if(!hasFinishedHalfTransition && currentTimeToTransition > timeToTransition * 0.5f) {
                hasFinishedHalfTransition = true;
                setState(stateToTransition);
            }

            if(currentTimeToTransition >= timeToTransition) {
                isTransitioning = false;
                hasFinishedHalfTransition = false;
                sceneAlpha = 1f;
                _graphics.setMaxAlpha(sceneAlpha);
            }
        }

        //Actualización de las animaciones de los botones del estado de elección de tamaño de tablero.
        if(currentState == GameState.BoardSizeMenu) {
            _goBackButton.update(deltaTime);
            for(Button b : _chooseSizeButtons) b.update(deltaTime);
        }

        //Actualización de las animaciones de los elementos del estado de juego.
        else if(currentState == GameState.Game) {
            _board.onUpdate(deltaTime);
            _reverseButton.update(deltaTime);
            _hintButton.update(deltaTime);
            _goBackButton.update(deltaTime);
        }
    }

    @Override
    public void onRender(Graphics graphics) {
        _graphics.setScaleAspect();
        //Pintar el estado del juego
        _graphics.clear(_clearColor);
        _graphics.translate(_graphics.getOffsetX(), _graphics.getOffsetY());
        _graphics.scale(_graphics.getLogicScaleAspect(), _graphics.getLogicScaleAspect());
        _graphics.save();

        _graphics.setMaxAlpha(sceneAlpha);

        if(currentState == GameState.MainMenu){
            //Tamaño Lógico: 400x600
            //Imagen Q42
            _graphics.translate(170, 400);
            _graphics.drawImage(_q43Img, 60, 90, 1.0f, false);

            //Textos
            _graphics.restore();
            _graphics.setColor(_black);
            _graphics.translate(200, 100);
            _graphics.setFont(_molleRegularTitle);
            _graphics.drawText("Oh no", 0, 0, true, true);

            _graphics.translate(0, 100 + _graphics.getTextHeight("0h no") * 0.5f);
            _graphics.setFont(_josefinSansTitle);
            _graphics.drawText("Jugar", 0, 0, true, false);

            _graphics.setColor(_grey);
            _graphics.translate(0, 80);
            _graphics.setFont(_josefinSansText);
            _graphics.drawText("Un juego copiado a Q42", 0,0, true, false);

            _graphics.translate(0, 20);
            _graphics.drawText("Creado por Martin Kool", 0, 0, true, false);
        }
        else if(currentState == GameState.BoardSizeMenu){
            //Texto
            _graphics.setColor(_black);
            if(!justSolvedBoard){
                _graphics.translate(200, 100);
                _graphics.setFont(_molleRegularTitle);
                _graphics.drawText("Oh no", 0, 0, true, true);
                _graphics.translate(0, 100);
            }
            else{
                _graphics.translate(200, 70);
                _graphics.setFont(_josefinSansTitle);
                _graphics.drawText("¡Maravilloso!", 0, 0, true, false);
                _graphics.translate(0, 130);
            }

            _graphics.setFont(_josefinSansText);
            _graphics.drawText("Escoge las dimensiones del tablero", 0, 0, true, false);

            //Imágenes
            _graphics.translate(0, 320);
            _graphics.drawImage(_goBackButton.getImage(), _goBackButton.getWidth() * _goBackButton.getScale(), _goBackButton.getHeight() * _goBackButton.getScale(),  _goBackButton.getAlpha(), true);

            //Botones de eleccion de tamaño de tablero
            _graphics.restore();
            _graphics.translate(100, 300);
            _graphics.save();
            _graphics.setFont(_josefinSansTitle);
            for(int k = 0; k < 3; k++) {
                if(k % 2 == 0) _graphics.setColor(_red);
                else _graphics.setColor(_blue);
                _graphics.fillCircle(0, 0,30 * _chooseSizeButtons[k].getScale(), 1.0f);
                _graphics.setColor(_clearColor);
                _graphics.drawText(Integer.toString(k + 4), 0,12, true, false);
                _graphics.translate(100, 0);
            }
            graphics.restore();
            graphics.translate(0, 70);
            for(int k = 0; k < 3; k++) {
                if(k % 2 == 1) _graphics.setColor(_red);
                else _graphics.setColor(_blue);
                _graphics.fillCircle(0, 0,30 * _chooseSizeButtons[k + 3].getScale(), 1.0f);
                _graphics.setColor(_clearColor);
                _graphics.drawText((Integer.toString(k + 7)),0, 12, true, false);
                _graphics.translate(100, 0);
            }
        }
        else if (currentState == GameState.Game){
            //Texto
            _graphics.setColor(_black);
            _graphics.translate(200, 70);
            if(_board.isSolved()){
                _graphics.setFont(_josefinSansTitle);
                _graphics.drawText("¡Maravilloso!", 0, 0, true, false);
            }
            else{
                String feedbackText = _board.get_boardFeedbackText();
                if(!feedbackText.isEmpty()){
                    _graphics.setFont(_josefinSansText);
                    _graphics.drawText(feedbackText, 0, 0, true, false);
                }
                else {
                    feedbackText = Integer.toString(boardSize) + " x " + Integer.toString(boardSize);
                    _graphics.setFont(_josefinSansTitle);
                    _graphics.drawText(feedbackText, 0, 0, true, false);
                }
            }

            _graphics.setFont(_josefinSansText);
            _graphics.restore();
            _graphics.save();
            _graphics.translate(200, 485);
            String percentageFilled = Integer.toString(_board.getPercentageFilled()) + "%";
            _graphics.drawText(percentageFilled, 0, 0, true, false);
            _graphics.restore();

            _graphics.translate(20, 100);
            _board.render();
            _graphics.translate(80, 420);

            //Botones
            _graphics.drawImage(_hintButton.getImage(), _hintButton.getWidth() * _hintButton.getScale(), _hintButton.getHeight() * _hintButton.getScale(), _hintButton.getAlpha(), true);
            _graphics.translate(100, 0);
            _graphics.drawImage(_goBackButton.getImage(), _goBackButton.getWidth() * _goBackButton.getScale(), _goBackButton.getHeight() * _goBackButton.getScale(),  _goBackButton.getAlpha(), true);
            _graphics.translate(100, 0);
            _graphics.drawImage(_reverseButton.getImage(), _reverseButton.getWidth() * _reverseButton.getScale(), _reverseButton.getHeight() * _reverseButton.getScale(),  _reverseButton.getAlpha(), true);
        }
    }

    private void transitionTowardsState(GameState newState){
        isTransitioning = true;
        hasFinishedHalfTransition = false;
        stateToTransition = newState;
        currentTimeToTransition = 0f;
    }

    private void setState(GameState newState){

        //Limpieza de estado actual
        if(currentState == GameState.Game) {
            justSolvedBoard = false;
            _board.clear();
        }

        //Cambiamos de estado
        currentState = newState;

        //Si no hemos visitado este estado anteriormente, hacemos new de todos los elementos que necesite
        if(!hasBeenGenerated[currentState.ordinal()]){
            if(newState == GameState.MainMenu){
                _q43Img = _graphics.newImage("q42.png");
                _playButton = new Button(145, 200,111, 60, null, 1, 1);
            }
            else if(newState == GameState.BoardSizeMenu){
                _goBackButton = new Button(180, 500, 40, 40, _graphics.newImage("close.png"), 0.5f, 1);
                _goBackButton.setScalingAnimation(1f, 1.1f, 0.3f, 1);
                int buttonHorizontalOffset = 40, buttonVerticalOffset = 70, buttonSize = 60;
                _chooseSizeButtons = new Button[6];
                for(int k = 0; k < 6; k++){
                    _chooseSizeButtons[k] = new Button(100 + (k % 3) * (buttonHorizontalOffset + buttonSize) - buttonSize/2, 300 +(k/3) * buttonVerticalOffset - buttonSize/2, buttonSize, buttonSize, _graphics.newImage("close.png"), 1, 1);
                    _chooseSizeButtons[k].setScalingAnimation(1f, 1.1f, 0.3f, 1);
                }
            }
            else{
                if(_goBackButton == null){ //Se genera en el estado anterior pero lo dejo por si al modificar la práctica hay que saltar del primer estado al juego.
                    _goBackButton = new Button(180, 500, 40, 40, _graphics.newImage("close.png"), 0.5f, 1);
                    _goBackButton.setScalingAnimation(1f, 1.1f, 0.3f, 1);
                }
                _board = new Board(_graphics, 360);
                _board.setPaintColors(_blue, _red, _lightgrey, _white, _black);
                _lockImg = _graphics.newImage("lock.png");

                _hintButton = new Button(80, 500, 40, 40, _graphics.newImage("eye.png"), 0.5f, 1);
                _hintButton.setScalingAnimation(1f, 1.1f, 0.3f, 1);
                _hintButton.setAnimationAlpha(0f, 0.5f, 0.3f);

                _reverseButton = new Button(280, 500, 40, 40, _graphics.newImage("history.png"), 0.5f, 1);
                _reverseButton.setScalingAnimation(1f, 1.1f, 0.3f, 1);
                _reverseButton.setAnimationAlpha(0f, 0.5f, 0.3f);
            }
        }

        //Inicialización de estados.
        if(currentState == GameState.Game) {
            _board.setSize(boardSize);
            _board.generate();
            _board.setFonts(_graphics.newFont("JosefinSans-Bold", 50.0f - (5.0f * (boardSize - 4)), true));
            _board.setButtons(20, 100);
        }
        hasBeenGenerated[currentState.ordinal()] = true;
    }
}