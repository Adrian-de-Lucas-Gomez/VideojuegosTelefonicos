package com.example.pcengine;

import com.example.engine.Application;
import com.example.engine.Engine;
import com.example.engine.Graphics;
import com.example.engine.Input;

import java.awt.Color;

public class PcEngine implements Engine {

    private PcGraphics _graphics;
    private PcInput _input;
    private Application _logic;
    private Window _window;
    private int _clearColor;
    private long _lastFrameTime = 0;

    public PcEngine(int width, int height){
        _window = new Window(width, height, 3);
        _graphics = new PcGraphics(_window);
        _input = new PcInput();
        _clearColor = 0xADD8E6;
    }

    @Override
    public PcGraphics getGraphics() {
        return _graphics;
    }

    @Override
    public PcInput getInput() {
        return _input;
    }

    @Override
    public void setApplication(Application a) {
        _logic = a;
    }

    public void run(){
        while(true){
            //Obtencion DeltaTime
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - _lastFrameTime;
            _lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;

            //Updates
            _graphics.clear(_clearColor);
            _logic.onUpdate(elapsedTime); //Revisar si tiene que ser double o es un poco extra
        }
    }
}