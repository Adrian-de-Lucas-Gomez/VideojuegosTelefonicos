package com.example.pcgame;

import com.example.engine.Application;
import com.example.pcengine.PcEngine;
import com.example.engine.Engine;
import com.example.logic.Logic;
import com.example.pcengine.PcGraphics;
import com.example.pcengine.Window;

public class PcGame {
    private PcEngine engine;
    public static void main(String[] args){
        PcEngine engine = new PcEngine(600, 600);
        Application a = new Logic();
        a.init(); //Revisar si hay que moverlo dentro del setApplication o cualquier otro lado.
        engine.setApplication(a);
        //Lanzariamos el bucle principal del motor
        engine.run();
    }
}