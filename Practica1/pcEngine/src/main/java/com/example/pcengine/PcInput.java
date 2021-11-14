package com.example.pcengine;

import com.example.engine.Graphics;
import com.example.engine.Input;
import com.example.engine.TouchEvent;

import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.util.ArrayList;

public class PcInput implements Input, MouseListener, MouseMotionListener {
    private ArrayList<TouchEvent> eventList;
    Graphics _graphics;

    public PcInput(Graphics graphics){
        _graphics = graphics;
        eventList = new ArrayList<TouchEvent>();
    }

    //Devuelve la lista de eventos
    @Override
    synchronized public ArrayList<TouchEvent> getTouchEvents(){
        if(eventList.size() > 0){
            ArrayList<TouchEvent> auxList = new ArrayList<TouchEvent>(eventList);
            eventList.clear();

            return auxList;
        }
        return eventList;
    }

    @Override
    public void mouseClicked(MouseEvent mouseEvent){
        //Calculo de coordenadas logicas
        int x = (int)((mouseEvent.getX() - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
        int y = (int)((mouseEvent.getY() - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());

        synchronized (this) {eventList.add(new TouchEvent(x, y, TouchEvent.EventType.buttonPressed));}
    }

    @Override
    public void mouseReleased(MouseEvent mouseEvent) {
        //Calculo de coordenadas logicas
        int x = (int)((mouseEvent.getX() - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
        int y = (int)((mouseEvent.getY() - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());

        synchronized (this) {eventList.add(new TouchEvent(x, y, TouchEvent.EventType.buttonReleased));}
    }

    //Es necesario definirlos pero como no se utilizan se dejan vacios
    @Override
    public void mousePressed(MouseEvent mouseEvent) { }
    @Override
    public void mouseEntered(MouseEvent mouseEvent) { }
    @Override
    public void mouseExited(MouseEvent mouseEvent) { }
    @Override
    public void mouseDragged(MouseEvent mouseEvent) { }
    @Override
    public void mouseMoved(MouseEvent mouseEvent) { }
}
