package com.example.pcengine;

import com.example.engine.Engine;

public class PcEngine implements Engine {
    PcGraphics graphics;
    private PcInput input;

    public PcGraphics getGraphics(){
        return graphics;
    }
    public PcInput getInput(){
        return input;
    }

    public void Run(){
        //Aqui iremos llamando a todos las partes del motor necesarias
        //y actualizaremos la lógica del juego
    }

}