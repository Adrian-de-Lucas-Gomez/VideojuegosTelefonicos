package com.example.androidengine;

import com.example.engine.Application;
import com.example.engine.Engine;

public class AndroidEngine implements Engine {
    private AndroidGraphics graphics;
    private AndroidInput input;

    public AndroidGraphics getGraphics(){
        return graphics;
    }
    public AndroidInput getInput(){
        return input;
    }

    @Override
    public void setApplication(Application a) {

    }

    public void run(){
        //Aqui iremos llamando a todos las partes del motor necesarias
        //y actualizaremos la lógica del juego
    }


}