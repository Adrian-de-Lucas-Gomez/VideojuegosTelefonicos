package com.example.engine;

import java.util.ArrayList;

public abstract class AbstractInput implements Input {
    //Lista en la que vamos a guardar los eventos que registremos
    protected ArrayList<TouchEvent> eventList;
    //Referencia al graphics del motor
    protected Graphics _graphics;

    /**
     * Devuelve la lista de eventos
     * Es sincronizado por si acediesen varias hebras a la vez que vayan de una en una
     */
    synchronized public ArrayList<TouchEvent> getTouchEvents(){
        if(eventList.size() > 0){
            ArrayList<TouchEvent> auxList = new ArrayList<TouchEvent>(eventList);
            eventList.clear();

            return auxList;
        }
        return eventList;
    }
}