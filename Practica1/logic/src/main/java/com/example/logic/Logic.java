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
    private Font _josefinSansButton;
    private Font _josefinSansText;

    //Menu state
    private Button _playButton;
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
        _red = new Color(255, 0, 0, 255);
        _grey = new Color(150, 150, 150, 255);
        _blue = new Color(0, 255, 255, 255);

        _q43Img = _graphics.newImage("q42.png"); //Tenemos que pasar ancho/alto

        _molleRegularTitle = _graphics.newFont("Molle-Regular", 100.0f, true);
        _josefinSansButton = _graphics.newFont("JosefinSans-Bold", 50.0f, true);
        _josefinSansText = _graphics.newFont("JosefinSans-Bold", 20.0f, false);
        setState(GameState.MainMenu);
    }

    @Override
    public void onDestroy() {

    }

    @Override
    public void onHandleInput() {
        for (TouchEvent e: _input.getTouchEvents()) {
            if(e.eventType == TouchEvent.EventType.buttonPressed){
                if(currentState == GameState.MainMenu){
                    if(_playButton.isPressed(e.posX - _graphics.getLogicOffsetX(), e.posY - _graphics.getLogicOffsetY())){
                        setState(GameState.BoardSizeMenu);
                    }
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
        /*
        _graphics.setColor(_black);
        _graphics.fillCircle(100, 100, 30);
        _graphics.setColor(new Color(0x000000));*/

        if(currentState == GameState.MainMenu){
            //Tamaño Lógico: 800x600
            _graphics.drawImage(_playButton.getImage(), _playButton.getX1(), _playButton.getY1(), _playButton.getX2() - _playButton.getX1(), _playButton.getY2() - _playButton.getY1());
            _graphics.setColor(_black);
            _graphics.drawText(_molleRegularTitle,"Oh no", 300, 200);
            _graphics.drawImage(_q43Img, 400, 500, 60, 60); //Debería estar en el centro y para abajo
            _graphics.drawText(_josefinSansButton, "Jugar", 380, 300);
            _graphics.setColor(_grey);
            _graphics.drawText(_josefinSansText, "Un juego copiado a Q42", 325, 340);
            _graphics.drawText(_josefinSansText, "Creado por Martin Kool", 330, 360);
        }
    }

    private void setState(GameState newState){
        currentState = newState;
        if(newState == GameState.MainMenu){
            Image img = _graphics.newImage("test.jpg");
            _playButton = new Button(100, 100,200, 200, img);
        }
    }
}