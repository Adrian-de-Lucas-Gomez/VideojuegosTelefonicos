package com.example.pcgame;

import com.example.engine.Application;
import com.example.engine.Engine;
import com.example.pcengine.PcEngine;
import com.example.logic.Logic;

public class PcGame {
    private PcEngine engine;

    public static void main(String[] args){

        Engine _engine = new PcEngine("0hn0", 600, 600);
        Application _app = new Logic(_engine);

        _app.init(); //Revisar si hay que moverlo dentro del setApplication o cualquier otro lado.
        _engine.setApplication(_app);

        //Bucle principal del motor
        _engine.run();
    }
}