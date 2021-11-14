package com.example.pcgame;

import com.example.engine.Application;
import com.example.engine.Engine;
import com.example.pcengine.PcEngine;
import com.example.logic.Logic;

public class PcGame {
    
    public static void main(String[] args){

        Engine _engine = new PcEngine("0hn0", 400, 600);
        Application _app = new Logic(_engine);

        _app.init();
        _engine.setApplication(_app);

        //Bucle principal del motor
        _engine.run();
    }
}