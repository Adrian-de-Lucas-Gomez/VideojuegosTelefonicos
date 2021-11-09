package com.example.androidengine;

import android.content.Context;
import android.os.Build;
import android.view.SurfaceView;

import androidx.annotation.RequiresApi;

import com.example.engine.Application;
import com.example.engine.Engine;

public class AndroidEngine implements Engine, Runnable {

    private Context context;
    private AndroidGraphics graphics;
    private AndroidInput input;
    private Application logic;

    //Para el input
    private SurfaceView surfaceView;

    public AndroidEngine(Context c){
        context = c;
        surfaceView = new SurfaceView(context);

        graphics = new AndroidGraphics(context);
        input = new AndroidInput();
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

        //getGraphics().setSurfaceView(surfaceView_);

        // esperamos a que surfaceView adquiera valor para poder hacer los calculos de reescalado
        while(getGraphics().getWindowWidth() == 0){}
        //getGraphics().scaleCanvas(); // ASPECT-RATIO

        while(true){    //Revisar

            //----------------------------------INPUT-------------------------------------
            //logic.onHandleInput();

            //----------------------------------UPDATE------------------------------------
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;
            //logic.onUpdate(elapsedTime);

            //----------------------------------RENDER------------------------------------
            getGraphics().startFrame(); // lock canvas

            graphics.clear(0x00ff00);
            //logic.onRender(graphics);
            getGraphics().endFrame();   // unlock canvas
        }
    }


}