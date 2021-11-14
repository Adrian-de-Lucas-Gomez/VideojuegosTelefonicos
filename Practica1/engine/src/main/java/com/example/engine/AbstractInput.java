package com.example.engine;

import java.util.ArrayList;

public abstract class AbstractInput implements Input {
    protected ArrayList<TouchEvent> eventList;
    protected Graphics _graphics;

    //Devuelve la lista de eventos
    synchronized public ArrayList<TouchEvent> getTouchEvents(){
        if(eventList.size() > 0){
            ArrayList<TouchEvent> auxList = new ArrayList<TouchEvent>(eventList);
            eventList.clear();

            return auxList;
        }
        return eventList;
    }
}
