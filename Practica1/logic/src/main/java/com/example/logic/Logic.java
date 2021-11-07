package com.example.logic;

import com.example.engine.Application;
import com.example.engine.Engine;
import com.example.engine.Graphics;
import com.example.engine.Image;
import com.example.engine.Input;
import com.example.engine.TouchEvent;

import java.awt.Color;
import java.awt.Graphics2D;

public class Logic implements Application {

    private Engine _engine;
    private Graphics _graphics;
    private Input _input;

    private int _clearColor = 0xADD8E6;
    private Color _redColor;

    //TODO pruebas
    private Image _imgPrueba;

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public Logic(Engine engine) {
        _engine = engine;
    }

    @Override
    public void init() {
        System.out.println("ESTOY VIVO");
        _graphics = _engine.getGraphics();
        _input = _engine.getInput();
        _redColor = new Color(0x8C31A0);
        _imgPrueba = _graphics.newImage("q42.png");
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
        _graphics.setColor(_redColor);
        _graphics.fillCircle(100, 100, 30);
        _graphics.drawImage(_imgPrueba, 100, 200, 50, 50);
    }
}