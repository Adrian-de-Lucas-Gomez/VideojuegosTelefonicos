package com.example.pcgame;

import com.example.pcengine.PcEngine;
import com.example.engine.Engine;
import com.example.logic.Logic;

public class PcGame {
    //No se si esta clase es la que deberia de implementar el main o tiene que ir suelto
    public void PcGame(){

    }

    public static void main(String[] args){
        PcEngine engine;
        Logic logic;

        engine = new PcEngine();
        logic = new Logic();

        //Lanzariamos el bucle principal del motor
        //engine.run();
    }
}