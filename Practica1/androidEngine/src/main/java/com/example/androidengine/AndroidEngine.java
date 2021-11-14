package com.example.androidengine;

import android.content.Context;
import android.view.SurfaceView;

import com.example.engine.Application;
import com.example.engine.Engine;

public class AndroidEngine implements Engine, Runnable {

    private Context context;
    private SurfaceView surfaceView; //Para coger el canvas a usar
    private AndroidGraphics graphics;
    private AndroidInput input;
    private Application logic;

    private Thread hilo; //Hilo de ejecución
    volatile private boolean _running;

    public AndroidEngine(Context c, int width, int height){
        context = c;
        surfaceView = new SurfaceView(context);
        graphics = new AndroidGraphics(context, width, height);
        input = new AndroidInput(graphics);
        surfaceView.setOnTouchListener(input); //Para que escuche los eventos que da el surface

        //Ponemos el juego a funcionar
        resume();
    }

    public AndroidGraphics getGraphics(){
        return graphics;
    }
    public AndroidInput getInput(){
        return input;
    }
    public SurfaceView getSurfaceView(){
        return surfaceView;
    }

    @Override
    public void setApplication(Application a) {
        logic = a;
    }

    public void run(){
        //Aqui iremos llamando a todos las partes del motor necesarias
        //y actualizaremos la lógica del juego

        long lastFrameTime = System.nanoTime();

        graphics.setSurfaceView(surfaceView);

        while(logic == null) {/*Esperamos al thread principal para que haga el setApplication*/}

        while(_running) {
            //Detectar cambios en el input
            logic.onHandleInput();

            //Zona para actualizar logica y DeltaTime
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;
            logic.onUpdate(elapsedTime);

            //Zona de Render
            graphics.lockCanvas();      //Bloquea el canvas
            logic.onRender(graphics);   //Graphics siendo el canvas donde pintamos
            graphics.releaseCanvas();   //Libera y pinta lo que hubiera en el canvas
        }
    }

    public void resume(){
        if(!_running){
            _running = true;
            hilo = new Thread(this);
            hilo.start();
        }
    }

    public void pause(){
        _running = false;
        while(true){
            try{
                hilo.join();
                hilo = null;
                break;
            }
            catch (InterruptedException e){
                System.out.println("No se pudo unir la hebra");
            }
        }
    }
}