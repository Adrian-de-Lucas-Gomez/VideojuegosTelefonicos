package com.example.androidengine;

import android.view.MotionEvent;
import android.view.View;

import com.example.engine.AbstractInput;
import com.example.engine.Graphics;
import com.example.engine.TouchEvent;

import java.util.ArrayList;

//Implementamos View.OnTouchListener para poder registrar eventos táctiles
public class AndroidInput extends AbstractInput implements View.OnTouchListener {

    public AndroidInput (Graphics graphics){
        super();
        _graphics = graphics;
        eventList = new ArrayList<TouchEvent>();
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
        int x = (int)((e.getX() - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
        int y = (int)((e.getY() - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());

        TouchEvent event = new TouchEvent(x,y, TouchEvent.EventType.buttonPressed);
        synchronized (this){eventList.add(event);}
    }

    //Retirar dedo de la pantalla
    synchronized public void touchRelease(MotionEvent e){
        //Transformamos las coordenadas del evento de posicion real en la pantalla a posicion lógica
        int x = (int)((e.getX() - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
        int y = (int)((e.getY() - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());

        TouchEvent event = new TouchEvent(x,y, TouchEvent.EventType.buttonReleased);
        synchronized (this){eventList.add(event);}
    }

    //Evento de arrastre en pantalla
    synchronized public void touchDragged(MotionEvent e){
        //Transformamos las coordenadas del evento de posicion real en la pantalla a posicion lógica
        int x = (int)((e.getX() - _graphics.getOffsetX()) / _graphics.getLogicScaleAspect());
        int y = (int)((e.getY() - _graphics.getOffsetY()) / _graphics.getLogicScaleAspect());
        
        TouchEvent event = new TouchEvent(x,y, TouchEvent.EventType.pointerMoved);
        synchronized (this){eventList.add(event);}
    }
}
