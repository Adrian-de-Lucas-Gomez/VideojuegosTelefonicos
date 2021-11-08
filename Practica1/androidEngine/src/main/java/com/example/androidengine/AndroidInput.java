package com.example.androidengine;

import com.example.engine.Input;
import com.example.engine.TouchEvent;

import java.util.ArrayList;
import java.util.List;

public class AndroidInput implements Input {
    private ArrayList<TouchEvent> events;
    public ArrayList<TouchEvent> getTouchEvents(){
        return events;
    }
}
