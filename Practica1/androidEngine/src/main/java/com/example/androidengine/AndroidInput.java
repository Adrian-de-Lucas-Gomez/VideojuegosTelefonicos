package com.example.androidengine;

import android.view.MotionEvent;
import android.view.View;

import com.example.engine.Graphics;
import com.example.engine.Input;
import com.example.engine.TouchEvent;

import java.util.ArrayList;
//Implementamos View.OnTouchListener para poder registrar eventos táctiles
public class AndroidInput implements Input , View.OnTouchListener {

    //Lista en la que vamos a guardar los eventos que registremos
    private ArrayList<TouchEvent> eventList;

    //Referencia al graphics del motor
    private Graphics _graphics;

    public AndroidInput (Graphics graphics){
        super();
        _graphics = graphics;
        eventList = new ArrayList<TouchEvent>();
    }

    //Devolvemos la lista de eventos. Es sincronizado por si acediesen varias hebras a la vez que vayan de una en una
    synchronized public ArrayList<TouchEvent> getTouchEvents(){
        if(eventList.size() > 0){
            ArrayList<TouchEvent> auxList = new ArrayList<TouchEvent>(eventList);
            eventList.clear();

            return auxList;
        }
        return eventList;
    }

    //Método que se dispara cada vez que se registra actividad el surface de Graphics
    @Override
    public boolean onTouch(View v, MotionEvent event) {

        //Comprobamos segun el tipo del evento registrado
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

    //Toque en la pantalla
    public void touchPressed(MotionEvent e){
        //Transformamos las coordenadas del evento de posicion real en la pantalla a posicion lógica
        int x= (int)((e.getX() - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
        int y= (int)((e.getY() - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());
        TouchEvent event = new TouchEvent(x,y, TouchEvent.EventType.buttonPressed);
        synchronized (this){eventList.add(event);}
    }

    //Retirar dedo de la pantalla
    synchronized public void touchRelease(MotionEvent e){
        int x= (int)((e.getX() - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
        int y= (int)((e.getY() - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());
        TouchEvent event = new TouchEvent(x,y, TouchEvent.EventType.buttonReleased);
        synchronized (this){eventList.add(event);}
    }

    //Evento de arrastre en pantalla
    synchronized public void touchDragged(MotionEvent e){
        int x= (int)((e.getX() - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
        int y= (int)((e.getY() - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());
        TouchEvent event = new TouchEvent(x,y, TouchEvent.EventType.pointerMoved);
        synchronized (this){eventList.add(event);}
    }
}
