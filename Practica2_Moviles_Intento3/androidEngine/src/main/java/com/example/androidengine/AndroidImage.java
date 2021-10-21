package com.example.androidengine;

import com.example.engine.Image;

public class AndroidImage implements Image {

    public AndroidImage(int w, int h){ //Supongo que habrá que poner el source
        width = w;
        height = h;
    }

    public int getWidth(){
        return width;
    }
    public int getHeight(){
        return height;
    }

    private int width;
    private int height;
}
