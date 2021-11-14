package com.example.androidengine;

import android.graphics.Typeface;
import com.example.engine.Font;

public class AndroidFont implements Font {

    private Typeface font;
    private float size;
    private boolean isBold;

    public AndroidFont(android.content.res.AssetManager manager, String name, float sz, boolean bold){
        font = Typeface.createFromAsset(manager, "fonts/"+name+".ttf");
        size = sz;
        isBold = bold;
    }

    //Propio de cada font ya que se usa para escribir el formato de la plataforma
    public Typeface getFont(){
        return font;
    }
    public float getSize(){
        return size;
    }
    public boolean isBold(){
        return isBold;
    }
}
