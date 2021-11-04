package com.example.pcengine;

import com.example.engine.Image;

public class PcImage implements Image {

    private java.awt.Image _img;

    public PcImage(java.awt.Image img){
        _img = img;
    }

    public int getWidth(){
        return _img.getWidth(null);
    }
    public int getHeight(){
        return _img.getHeight(null);
    }
}
