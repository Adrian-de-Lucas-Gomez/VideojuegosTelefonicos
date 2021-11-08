package com.example.pcengine;

import com.example.engine.Input;
import com.example.engine.TouchEvent;

import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.util.ArrayList;
import java.util.List;

public class PcInput implements Input, MouseListener, MouseMotionListener {
    private ArrayList<TouchEvent> eventList;

    public PcInput(){
        eventList = new ArrayList<TouchEvent>();
    }

    @Override
    public ArrayList<TouchEvent> getTouchEvents(){
        if(eventList.size() > 0){
            ArrayList<TouchEvent> auxList = new ArrayList<TouchEvent>(eventList);
            eventList.clear();

            return auxList;
        }
        return eventList;
    }

    @Override
    public void mouseClicked(MouseEvent mouseEvent){
        eventList.add(new TouchEvent(mouseEvent.getX(), mouseEvent.getY(), TouchEvent.EventType.buttonPressed));
    }

    @Override
    public void mouseReleased(MouseEvent mouseEvent) {
        eventList.add(new TouchEvent(mouseEvent.getX(), mouseEvent.getY(), TouchEvent.EventType.buttonReleased));
    }

    //Es necesario definirlos pero como no se utilizan se dejan vacíos
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
