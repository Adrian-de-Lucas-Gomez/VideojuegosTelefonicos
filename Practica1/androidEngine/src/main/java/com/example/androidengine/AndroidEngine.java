package com.example.androidengine;

import android.content.Context;
import android.view.SurfaceView;

import com.example.engine.Application;
import com.example.engine.Engine;
import com.example.engine.Font;

public class AndroidEngine implements Engine, Runnable {

    private Context context;
    private AndroidGraphics graphics;
    private AndroidInput input;
    private Application logic;

    //Hilo de ejecución
    private Thread hilo;

    volatile private boolean _running;

    //Para pillar el canvas a usar
    private SurfaceView surfaceView;

    public AndroidEngine(Context c, int width, int height){
        context = c;
        surfaceView = new SurfaceView(context);

        graphics = new AndroidGraphics(context, width, height);
        input = new AndroidInput();

        //Ponemos el juego a funcionar
        _running = true;
        //Revisar
        hilo = new Thread(this);
        hilo.start();
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

        //Test
        //Font aux= graphics.newFont("JosefinSans-Bold", 20.0f, false);
        //graphics.setFont(aux);

        while(_running){    //Revisar

            //Detectar cambios en el input
            //logic.onHandleInput();

            //Zona para actualizar logica y DeltaTime
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;
            //logic.onUpdate(elapsedTime);


            //Deberiamos tener esto en un método Render por si le damos una thread?????
            //Zona de Render
            graphics.lockCanvas(); // Bloquea el canvas
            //graphics.clear(0x00ff00);   //Debug
            logic.onRender(graphics);     //Graphics siendo el canvas donde pintamos
            graphics.releaseCanvas();   // Libera y pinta lo que hubiera en el canvas

            //---------------------------------------------------------------------------------------
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