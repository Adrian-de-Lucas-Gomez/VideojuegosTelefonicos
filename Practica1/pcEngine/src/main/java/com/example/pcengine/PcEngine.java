package com.example.pcengine;

import com.example.engine.Application;
import com.example.engine.Engine;

import java.awt.Graphics2D;
import java.awt.List;
import java.awt.event.ComponentAdapter;
import java.awt.event.ComponentEvent;

public class PcEngine implements Engine {

    private Window _window;
    private PcGraphics _graphics;
    private PcInput _input;
    private Application _logic;

    private long _lastFrameTime = 0;

    public PcEngine(String title, int width, int height){
        _window = new Window(title, width, height, 2);
        _graphics = new PcGraphics(_window, width, height);
        _input = new PcInput();
        _window.addMouseListener(_input);
        _window.addMouseMotionListener(_input);
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
            _logic.onHandleInput();
            //_window.getBufferStrategy().show();

            //Ahora permite redimensionar pero el valor de tamaño de la ventana no cambia
            //y si se hace grande se ve vacio mas allá del clear. La imagen desaparece porque yolo
            do {
                do {
                    java.awt.Graphics2D graphicsJava = (Graphics2D)_window.getBufferStrategy().getDrawGraphics();
                    _graphics.setGraphics(graphicsJava);
                    _graphics.setScaleAspect(); //TODO no sé si va aquí
                    try {
                        //Se llama a pintar lo que toque
                        _logic.onRender(_graphics);
                    }
                    finally {
                        graphicsJava.dispose();
                    }
                } while(_window.getBufferStrategy().contentsRestored());
                _window.getBufferStrategy().show(); //Mandamos a pintar en pantalla
            } while(_window.getBufferStrategy().contentsLost());
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