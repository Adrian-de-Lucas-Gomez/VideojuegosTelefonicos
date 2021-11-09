package com.example.androidengine;

import android.graphics.Bitmap;

import com.example.engine.Image;

public class AndroidImage implements Image {

    private Bitmap _img;

    public AndroidImage(Bitmap img){
        _img = img;
    }

    public int getWidth(){
        return _img.getWidth();
    }
    public int getHeight() {
        return _img.getHeight();
    }
    public Bitmap getImage(){
        return _img;
    }
}
