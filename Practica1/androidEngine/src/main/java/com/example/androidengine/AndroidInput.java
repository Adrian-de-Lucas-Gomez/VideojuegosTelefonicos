package com.example.androidengine;

import com.example.engine.Input;
import com.example.engine.TouchEvent;

import java.util.List;

public class AndroidInput implements Input {
    private List<TouchEvent> events;
    public List<TouchEvent> getTouchEvents(){
        return events;
    }
}
