package com.example.pcengine;

import com.example.engine.Application;
import com.example.engine.Engine;

import java.awt.Graphics2D;

public class PcEngine implements Engine {

    private Window _window;
    private PcGraphics _graphics;
    private PcInput _input;
    private Application _logic;

    private long _lastFrameTime = 0;

    public PcEngine(String title, int width, int height){
        _window = new Window(title, width, height, 2); //2 buffers para pintar
        _graphics = new PcGraphics(_window, width, height);

        //Para input
        _input = new PcInput(_graphics);
        _window.addMouseListener(_input);
        _window.addMouseMotionListener(_input);
    }

    public void run() {

        while(true){
            //Obtencion DeltaTime
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - _lastFrameTime;
            _lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;

            //Updates
            _logic.onUpdate(elapsedTime);
            _logic.onHandleInput();

            //Renderizado
            do {
                do {
                    java.awt.Graphics2D graphicsJava = (Graphics2D)_window.getBufferStrategy().getDrawGraphics();
                    _graphics.setGraphics(graphicsJava);
                    try {
                        _logic.onRender(_graphics); //Se llama a pintar lo que toque
                    }
                    finally {
                        graphicsJava.dispose();
                    }
                } while(_window.getBufferStrategy().contentsRestored());
                _window.getBufferStrategy().show(); //Mandamos a pintar en pantalla
            } while(_window.getBufferStrategy().contentsLost());
        }
    }

    //+++++++++++++GETTERS Y SETTERS+++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //Asignar la lógica del juego al motor
    @Override
    public void setApplication(Application a) {
        _logic = a;
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