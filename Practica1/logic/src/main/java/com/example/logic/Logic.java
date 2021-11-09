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

    private Color _clearColor, _black, _red, _grey, _blue;

    private GameState currentState;

    //TODO pruebas
    private Image _q43Img;
    private Font _molleRegularTitle;
    private Font _josefinSansTitle;
    private Font _josefinSansText;

    //Menu state
    private Button _playButton;
    private float _playButtonLength;

    //ChooseBoardSize state
    private Button _goToTitleButton;
    private Button[] _chooseSizeButtons;
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

        _q43Img = _graphics.newImage("q42.png");

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
            int logicX = e.posX, logicY = e.posY;
            if(e.eventType == TouchEvent.EventType.buttonPressed){
                if(currentState == GameState.MainMenu){
                    if(_playButton.isPressed(logicX, logicY)) setState(GameState.BoardSizeMenu);
                }
                else if(currentState == GameState.BoardSizeMenu){
                    if (_goToTitleButton.isPressed(logicX, logicY)) setState(GameState.MainMenu);
                    else{
                        for(int k = 0; k < _chooseSizeButtons.length; k++){
                            if(_chooseSizeButtons[k].isPressed(logicX, logicY)){
                                setState(GameState.Game);
                                break;
                            }
                        }
                    }
                }
                else if (currentState == GameState.Game){
                    if (_goToTitleButton.isPressed(logicX, logicY)) setState(GameState.MainMenu);
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
        _graphics.scale(_graphics.getLogicScaleAspect(), _graphics.getLogicScaleAspect());
        _graphics.translate(_graphics.getOffsetX(), _graphics.getOffsetY());
        _graphics.clear(_clearColor.getRGB());
        _graphics.save();

        if(currentState == GameState.MainMenu){
            //Tamaño Lógico: 800x600
            //_graphics.drawImage(_playButton.getImage(), _playButton.getX(), _playButton.getY(), _playButton.getWidth(), _playButton.getHeight());
            //Imagen Q42
            _graphics.translate(200-30, 400-40);
            _graphics.drawImage(_q43Img, 60, 80);

            //Textos
            _graphics.restore();
            _graphics.setColor(_black.getRGB());
            _graphics.translate(100, 100);
            _graphics.drawText(_molleRegularTitle,"Oh no", 10, 10);
            _graphics.translate(80, 100);
            _graphics.drawText(_josefinSansTitle, "Jugar", 0, 0);

            _graphics.restore();
            _graphics.setColor(_grey.getRGB());
            _graphics.translate(125, 340);
            _graphics.drawText(_josefinSansText, "Un juego copiado a Q42", 0, 0);
            _graphics.restore();
            _graphics.translate(130, 360);
            _graphics.drawText(_josefinSansText, "Creado por Martin Kool", 0, 0);
        }
        else if(currentState == GameState.BoardSizeMenu){
            _graphics.setColor(_black.getRGB());
            _graphics.drawText(_molleRegularTitle,"Oh no", 300, 200);
            //_graphics.drawImage(_goToTitleButton.getImage(), _goToTitleButton.getX(), _goToTitleButton.getY(), _goToTitleButton.getWidth(), _goToTitleButton.getHeight());
            for(int k = 0; k < _chooseSizeButtons.length; k++) {
                if(k % 2 == 0) _graphics.setColor(_red.getRGB());
                else _graphics.setColor(_blue.getRGB());
                _graphics.fillCircle(_chooseSizeButtons[k].getX() + _chooseSizeButtons[k].getWidth() / 2, _chooseSizeButtons[k].getY() + _chooseSizeButtons[k].getHeight() / 2, _chooseSizeButtons[k].getWidth() / 2);
                //_graphics.drawImage(_chooseSizeButtons[k].getImage(), _chooseSizeButtons[k].getX(), _chooseSizeButtons[k].getY(), _chooseSizeButtons[k].getWidth(), _chooseSizeButtons[k].getHeight());
                _graphics.setColor(_clearColor.getRGB());
                _graphics.drawText(_josefinSansTitle, Integer.toString(k + 4), _chooseSizeButtons[k].getX() + _chooseSizeButtons[k].getWidth() / 2 - 12 /*ESTO ES TOTALMENTE ARBITRARIO*/,  _chooseSizeButtons[k].getY() + _chooseSizeButtons[k].getHeight() / 2 + 20);
            }
        }
        else if (currentState == GameState.Game){
            //_graphics.drawImage(_goToTitleButton.getImage(), _goToTitleButton.getX(), _goToTitleButton.getY(), _goToTitleButton.getWidth(), _goToTitleButton.getHeight());
        }
    }

    private void setState(GameState newState){
        currentState = newState;
        if(newState == GameState.MainMenu){
            _playButton = new Button(380, 260,120, 50, null);
        }
        else if(newState == GameState.BoardSizeMenu){
            _goToTitleButton = new Button(400, 500, 40, 40, _graphics.newImage("close.png"));
            int buttonHorizontalOffset = 30, buttonVerticalOffset = 80, buttonSize = 60;
            _chooseSizeButtons = new Button[6];
            for(int k = 0; k < 6; k++){
                _chooseSizeButtons[k] = new Button(300 + (k % 3) * (buttonHorizontalOffset + buttonSize), 300 +(k/3) * buttonVerticalOffset, buttonSize, buttonSize, _graphics.newImage("close.png"));
            }
        }
    }
}