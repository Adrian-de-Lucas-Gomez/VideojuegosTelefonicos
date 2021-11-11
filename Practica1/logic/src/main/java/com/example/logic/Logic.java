package com.example.logic;

import com.example.engine.Application;
import com.example.engine.Button;
import com.example.engine.Engine;
import com.example.engine.Font;
import com.example.engine.Graphics;
import com.example.engine.Image;
import com.example.engine.Input;
import com.example.engine.TouchEvent;

import java.awt.Color;

public class Logic implements Application {

    private enum GameState{
        MainMenu, BoardSizeMenu, Game
    }

    private Engine _engine;
    private Graphics _graphics;
    private Input _input;

    private Color _clearColor, _black, _red, _grey, _blue, _white;

    private GameState currentState;

    //TODO pruebas
    private Image _q43Img;
    private Image _testImg;
    private Font _molleRegularTitle;
    private Font _josefinSansTitle;
    private Font _josefinSansText;

    //Menu state
    private Button _playButton;
    private float _playButtonLength;

    //ChooseBoardSize state
    private Button _goToTitleButton;
    private Button[] _chooseSizeButtons;

    //Game state
    private int boardSize = 0;
    private Board _board;
    private Image _lockImg;
    private Button _hintButton, _reverseButton;
    private String _hintText = "";
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public Logic(Engine engine) {
        _engine = engine;
    }

    @Override
    public void init() {
        _graphics = _engine.getGraphics();
        _input = _engine.getInput();

        _clearColor = new Color(220, 220, 220, 255);
        _black = new Color(0, 0, 0, 255);
        _red = new Color(255, 20, 20, 255);
        _grey = new Color(150, 150, 150, 255);
        _blue = new Color(0, 100, 255, 255);
        _white = new Color(255, 255, 255, 255);

        _q43Img = _graphics.newImage("q42.png");
        _testImg = _graphics.newImage("test.jpg");
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
        for (TouchEvent e: _input.getTouchEvents()) {
            int pointerX = (int)((e.posX - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
            int pointerY = (int)((e.posY - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());
            System.out.println("Pointer: " + Integer.toString(pointerX) + ", " + Integer.toString(pointerY));
            if(e.eventType == TouchEvent.EventType.buttonPressed){
                if(currentState == GameState.MainMenu){
                    if(_playButton.isPressed(pointerX, pointerY)) setState(GameState.BoardSizeMenu);
                }
                else if(currentState == GameState.BoardSizeMenu){
                    if (_goToTitleButton.isPressed(pointerX, pointerY)) setState(GameState.MainMenu);
                    else{
                        for(int k = 0; k < _chooseSizeButtons.length; k++){
                            if(_chooseSizeButtons[k].isPressed(pointerX, pointerY)){
                                boardSize = k + 4;
                                setState(GameState.Game);
                                break;
                            }
                        }
                    }
                }
                else if (currentState == GameState.Game){
                    if(_board.handleInput(pointerX, pointerY)) _hintText = ""; //TODO revisar esto que devuelva un boolean. es horrible
                    if (_goToTitleButton.isPressed(pointerX, pointerY)) setState(GameState.MainMenu);
                    else if(_hintButton.isPressed(pointerX, pointerY)) _hintText = _board.getHintText();
                    else if(_reverseButton.isPressed(pointerX, pointerY)) _board.revertPlay();
                }
            }
        }
    }

    @Override
    public void onUpdate(double deltaTime) {
    }

    @Override
    public void onRender(Graphics graphics) {
        //Pintar el estado del juego
        _graphics.clear(_clearColor.getRGB());
        _graphics.translate(_graphics.getOffsetX(), _graphics.getOffsetY());
        _graphics.scale(_graphics.getLogicScaleAspect(), _graphics.getLogicScaleAspect());
        _graphics.save();

        int textWidth, textHeight;

        if(currentState == GameState.MainMenu){
            //Tamaño Lógico: 400x600
            //Imagen Q42
            _graphics.translate(170, 360);
            _graphics.drawImage(_q43Img, 60, 80);

            //Textos
            _graphics.restore();
            _graphics.setColor(_black);
            _graphics.translate(200, 0);
            textHeight = _graphics.getTextHeight(_molleRegularTitle, "Oh no");
            textWidth = _graphics.getTextWidth(_molleRegularTitle, "Oh no");
            _graphics.drawText(_molleRegularTitle,"Oh no", -textWidth/2, textHeight);
            _graphics.translate(0, 100 + textHeight);
            textWidth = _graphics.getTextWidth(_josefinSansTitle, "Jugar");
            textHeight = _graphics.getTextHeight(_josefinSansTitle, "Jugar");
            _graphics.drawText(_josefinSansTitle, "Jugar", -textWidth/2, 0);
            _graphics.setColor(_grey);
            _graphics.translate(0, 80);
            textWidth = _graphics.getTextWidth(_josefinSansText, "Un juego copiado a Q42");
            _graphics.drawText(_josefinSansText, "Un juego copiado a Q42", -textWidth/2, 0);
            _graphics.translate(0, 20);
            textWidth = _graphics.getTextWidth(_josefinSansText, "Creado por Martin Kool");
            _graphics.drawText(_josefinSansText, "Creado por Martin Kool", -textWidth/2, 0);
        }
        else if(currentState == GameState.BoardSizeMenu){
            //Texto
            _graphics.setColor(_black);
            _graphics.translate(200, 0);
            textHeight = _graphics.getTextHeight(_molleRegularTitle, "Oh no");
            textWidth = _graphics.getTextWidth(_molleRegularTitle, "Oh no");
            _graphics.drawText(_molleRegularTitle,"Oh no", -textWidth/2, textHeight);
            _graphics.translate(0, 200);
            textWidth = _graphics.getTextWidth(_josefinSansText, "Escoge las dimensiones del tablero.");
            _graphics.drawText(_josefinSansText,"Escoge las dimensiones del tablero.", -textWidth/2, 0);
            //Imágenes
            _graphics.translate(-_goToTitleButton.getWidth()/2, 300);
            _graphics.drawImage(_goToTitleButton.getImage(), _goToTitleButton.getWidth(), _goToTitleButton.getHeight());

            //Botones de eleccion de tamaño de tablero
            _graphics.restore();
            _graphics.translate(100, 300);
            _graphics.save();
            for(int k = 0; k < 3; k++) {
                if(k % 2 == 0) _graphics.setColor(_red);
                else _graphics.setColor(_blue);
                _graphics.fillCircle(0, 0,30);
                _graphics.setColor(_clearColor);
                _graphics.drawText(_josefinSansTitle, Integer.toString(k + 4),
                    - _graphics.getTextWidth(_josefinSansTitle, Integer.toString(k + 4)) / 2, _graphics.getTextHeight(_josefinSansTitle, Integer.toString(k + 4)) / 3);
                _graphics.translate(100, 0);
            }
            graphics.restore();
            graphics.translate(0, 70);
            for(int k = 0; k < 3; k++) {
                if(k % 2 == 1) _graphics.setColor(_red);
                else _graphics.setColor(_blue);
                _graphics.fillCircle(0, 0,30);
                _graphics.setColor(_clearColor);
                _graphics.drawText(_josefinSansTitle, Integer.toString(k + 7),
                    - _graphics.getTextWidth(_josefinSansTitle, Integer.toString(k + 7)) / 2, _graphics.getTextHeight(_josefinSansTitle, Integer.toString(k + 7)) / 3);
                _graphics.translate(100, 0);
            }
        }
        else if (currentState == GameState.Game){
            //Texto
            _graphics.setColor(_black);
            if(_hintText != ""){
                _graphics.translate(200, 70);
                _graphics.drawText(_josefinSansText, _hintText, -_graphics.getTextWidth(_josefinSansText, _hintText)/2, 0);
                _graphics.restore();
            }

            _graphics.translate(200, 485);
            String aux = Integer.toString(_board.getPercentageFilled()) + "%";
            _graphics.drawText(_josefinSansText, aux, -_graphics.getTextWidth(_josefinSansText, aux)/2, 0);
            _graphics.restore();

            _graphics.translate(20, 100);
            _board.paint();
            _graphics.translate(60, 400);

            //Botones
            _graphics.drawImage(_hintButton.getImage(), _hintButton.getWidth(), _hintButton.getHeight());
            _graphics.translate(100, 0);
            _graphics.drawImage(_goToTitleButton.getImage(), _goToTitleButton.getWidth(), _goToTitleButton.getHeight());
            _graphics.translate(100, 0);
            _graphics.drawImage(_reverseButton.getImage(), _reverseButton.getWidth(), _reverseButton.getHeight());
        }
    }

    private void setState(GameState newState){
        //TODO: Reciclar los objetos (Que solo se generen la primera vez que se cambia al nuevo estado)
        currentState = newState;
        if(newState == GameState.MainMenu){
            _playButton = new Button(145, 200,111, 60, null);
        }
        else if(newState == GameState.BoardSizeMenu){
            //126, 300
            _goToTitleButton = new Button(180, 500, 40, 40, _graphics.newImage("close.png"));
            int buttonHorizontalOffset = 40, buttonVerticalOffset = 70, buttonSize = 60;
            _chooseSizeButtons = new Button[6];
            for(int k = 0; k < 6; k++){
                _chooseSizeButtons[k] = new Button(100 + (k % 3) * (buttonHorizontalOffset + buttonSize) - buttonSize/2, 300 +(k/3) * buttonVerticalOffset - buttonSize/2, buttonSize, buttonSize, _graphics.newImage("close.png"));
            }
        }
        else{
            _board = new Board(boardSize, _graphics, 360);
            _board.setPaintColors(_blue, _red, _grey, _white, _black);
            _board.setFonts(_graphics.newFont("JosefinSans-Bold", 50.0f - (5.0f * (boardSize - 4)), true), _molleRegularTitle);
            _board.setButtons(20, 100);
            _board.generate();
            _hintText = "";
            _lockImg = _graphics.newImage("lock.png");

            _hintButton = new Button(80, 500, 40, 40, _graphics.newImage("eye.png"));
            _reverseButton = new Button(280, 500, 40, 40, _graphics.newImage("history.png"));
        }
    }
}