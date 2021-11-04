package com.example.androidengine;

import com.example.engine.Engine;

public class AndroidEngine implements Engine {
    AndroidGraphics graphics;
    private AndroidInput input;

    public AndroidGraphics getGraphics(){
        return graphics;
    }
    public AndroidInput getInput(){
        return input;
    }

    public void Run(){
        //Aqui iremos llamando a todos las partes del motor necesarias
        //y actualizaremos la lógica del juego
    }


}