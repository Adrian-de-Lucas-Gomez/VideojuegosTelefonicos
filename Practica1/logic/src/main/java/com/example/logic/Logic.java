package com.example.logic;

import com.example.engine.Application;
import com.example.engine.Engine;
import com.example.engine.Font;
import com.example.engine.Graphics;
import com.example.engine.Image;
import com.example.engine.Input;
import com.example.engine.TouchEvent;

import java.awt.Color;

public class Logic implements Application {

    private Engine _engine;
    private Graphics _graphics;
    private Input _input;

    private int _clearColor = 0xADD8E6;
    private Color _color;

    //TODO pruebas
    private Image _imgPrueba;
    private Font _fontPrueba;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public Logic(Engine engine) {
        _engine = engine;
    }

    @Override
    public void init() {
        System.out.println("ESTOY VIVO");
        _graphics = _engine.getGraphics();
        _input = _engine.getInput();
        _color = new Color(0x8C31A0);
        _imgPrueba = _graphics.newImage("q42.png");
        _fontPrueba = _graphics.newFont("Molle-Regular", 52.0f, true);
    }

    @Override
    public void onDestroy() {

    }

    @Override
    public void onEvent(TouchEvent event) {

    }

    @Override
    public void onUpdate(double deltaTime) {
        //Logica de numeritos
    }

    @Override
    public void onRender(Graphics graphics) {
        //Pintar el estado del juego
        _graphics.clear(_clearColor);
        _graphics.setColor(_color);
        _graphics.fillCircle(100, 100, 30);
        _graphics.setColor(_color);
        _graphics.drawText(_fontPrueba,"Ole ole los caracoles", 50, 350);
        _graphics.drawText(_fontPrueba,"Muerte a PePa", 69, 420);
        _graphics.drawImage(_imgPrueba, 100, 200, 50, 65);

        //TODO Pruebas
        _graphics.setColor(new Color(0,0,0));
        _graphics.fillCircle(10, 10, 5);
        _graphics.fillCircle(10, _graphics.getGameHeight() - 10, 5);
        _graphics.fillCircle(_graphics.getGameWidth() - 10, 10, 5);
        _graphics.fillCircle(_graphics.getGameWidth() - 10, _graphics.getGameHeight() - 10, 5);
    }
}