package com.example.androidengine;

import android.graphics.Typeface;
import com.example.engine.Font;

public class AndroidFont implements Font {

    Typeface font;

    public AndroidFont(android.content.res.AssetManager manager, String name){
        font = Typeface.createFromAsset(manager, "./assets/fonts/"+name+".ttf");

        //font.
    }
    //Propio de cada font ya que se usa para escribir el formato de la plataforma
    public Typeface getFont(){
        return font;
    }
}
