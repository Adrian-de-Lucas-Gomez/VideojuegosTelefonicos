package com.example.pcengine;

import com.example.engine.Application;
import com.example.engine.Engine;

public class PcEngine implements Engine {

    private Window _window;
    private PcGraphics _graphics;
    private PcInput _input;
    private Application _logic;

    private long _lastFrameTime = 0;

    public PcEngine(String title, int width, int height){
        _window = new Window(title, width, height, 2);
        _graphics = new PcGraphics(_window);
        _input = new PcInput();
    }

    @Override
    public void setApplication(Application a) {
        _logic = a;
    }

    public void run(){ //TODO mover lo que no haga falta aqui
        while(true){

            //Obtencion DeltaTime
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - _lastFrameTime;
            _lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;

            //TODO arreglar esto
            //Updates
            _logic.onUpdate(elapsedTime); //Revisar si tiene que ser double o es un poco extra
            _window.getBufferStrategy().show();
            
            /*
            do {
                do {
                    Graphics graphics = strategy.getDrawGraphics();
                    try {
                        _window.render(graphics);
                    }
                    finally {
                        graphics.dispose();
                    }
                } while(strategy.contentsRestored());
                strategy.show();
            } while(strategy.contentsLost());
            */
        }
    }

    @Override
    public PcGraphics getGraphics() {
        return _graphics;
    }

    @Override
    public PcInput getInput() {
        return _input;
    }
}