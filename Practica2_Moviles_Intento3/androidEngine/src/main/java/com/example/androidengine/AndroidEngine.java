package com.example.androidengine;

import com.example.engine.Engine;

public class AndroidEngine implements Engine {
    AndroidGraphics graphics;


    public AndroidGraphics getGraphics(){
        return graphics;
    }
    public AndroidInput getInput(){
        return input;
    }

    private AndroidInput input;
}