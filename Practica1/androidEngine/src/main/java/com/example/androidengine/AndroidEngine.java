package com.example.androidengine;

import android.content.Context;
import android.view.SurfaceView;

import com.example.engine.Application;
import com.example.engine.Engine;

//Hereda de Runnable para poder hacer ejecucion concurrente
public class AndroidEngine implements Engine, Runnable {

    private Context context;            //Contexto de la aplicacion
    private SurfaceView surfaceView;    //Para coger el canvas a usar
    private AndroidGraphics graphics;   //Modulo de graficos del motor
    private AndroidInput input;         //Modulo de graficos del motor
    private Application logic;          //Modulo de graficos del motor

    private Thread hilo; //Hilo de ejecución

    //Variable de control de la ejecucion (Volatil para evitar problemas con las hebras)
    volatile private boolean _running;

    public AndroidEngine(Context c, int width, int height){
        context = c;
        surfaceView = new SurfaceView(context);     //Sacamos la surface del contaxto de la Aplicacion android

        //Inicializamos los modulos
        graphics = new AndroidGraphics(context, width, height);
        input = new AndroidInput(graphics);
        surfaceView.setOnTouchListener(input); //Para que escuche los eventos que da el surface

        //Ponemos el juego a funcionar
        resume();
    }

    //Getters de los modulos y el surfaceView
    public AndroidGraphics getGraphics(){
        return graphics;
    }
    public AndroidInput getInput(){
        return input;
    }
    public SurfaceView getSurfaceView(){
        return surfaceView;
    }

    //Metodo para establecer la referencia a la lógica usando la interfaz Application
    @Override
    public void setApplication(Application a) {
        logic = a;
    }

    //Bucle principal de la ejecución que se ejecuta en un thread diferente al principal
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

            //Logica
            logic.onUpdate(elapsedTime);

            //Zona de Render
            graphics.lockCanvas();      //Bloquea el canvas
            logic.onRender(graphics);   //Graphics siendo el canvas donde pintamos
            graphics.releaseCanvas();   //Libera y pinta lo que hubiera en el canvas
        }
    }

    //Metodo para reanudar o iniciar por primera ver la thread del runnable
    public void resume(){
        if(!_running){
            _running = true;
            hilo = new Thread(this);
            hilo.start();
        }
    }

    //Metodo para parar la thread en el caso de perder el foco o minimizar la aplicación
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