package com.example.androidengine;

import android.view.MotionEvent;
import android.view.View;

import com.example.engine.Graphics;
import com.example.engine.Input;
import com.example.engine.TouchEvent;

import java.util.ArrayList;

public class AndroidInput implements Input , View.OnTouchListener {
    private ArrayList<TouchEvent> eventList;
    private Graphics _graphics;

    public AndroidInput (Graphics graphics){
        super();
        _graphics = graphics;
        eventList = new ArrayList<TouchEvent>();
    }

    synchronized public ArrayList<TouchEvent> getTouchEvents(){
        if(eventList.size() > 0){
            ArrayList<TouchEvent> auxList = new ArrayList<TouchEvent>(eventList);
            eventList.clear();

            return auxList;
        }
        return eventList;
    }

    @Override
    public boolean onTouch(View v, MotionEvent event) {
        if(event.getActionMasked() == MotionEvent.ACTION_DOWN || event.getActionMasked() == MotionEvent.ACTION_POINTER_DOWN){
            touchPressed(event);
        }
        else if(event.getActionMasked() == MotionEvent.ACTION_UP || event.getActionMasked() == MotionEvent.ACTION_POINTER_UP){
            touchRelease(event);
        }
        else if(event.getActionMasked() == MotionEvent.ACTION_MOVE){
            touchDragged(event);
        }
        return true;
    }

    public void touchPressed(MotionEvent e){
        int x= (int)((e.getX() - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
        int y= (int)((e.getY() - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());
        TouchEvent event = new TouchEvent(x,y, TouchEvent.EventType.buttonPressed);
        synchronized (this){eventList.add(event);}
    }

    synchronized public void touchRelease(MotionEvent e){
        int x= (int)((e.getX() - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
        int y= (int)((e.getY() - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());
        TouchEvent event = new TouchEvent(x,y, TouchEvent.EventType.buttonReleased);
        synchronized (this){eventList.add(event);}
    }

    synchronized public void touchDragged(MotionEvent e){
        int x= (int)((e.getX() - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
        int y= (int)((e.getY() - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());
        TouchEvent event = new TouchEvent(x,y, TouchEvent.EventType.pointerMoved);
        synchronized (this){eventList.add(event);}
    }
}
